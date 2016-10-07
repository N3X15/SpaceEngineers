import logging
import os
import re
import shutil
import sys

import yaml
from lxml import etree

# CONFIGURATION

# Guess.
SE_ROOT_DIR = os.path.abspath(sys.argv[1])

REG_FIND_BAD_SELFCLOSERS = re.compile(r'([^ ])/>')
FIX_BAD_SELFCLOSERS = '\\1 />'

MSBNS = '{http://schemas.microsoft.com/developer/msbuild/2003}'
class Reference:
    def __init__(self, refXML=None):
        self.Include = ''
        self.Private=None
        self.HintPath=None
        if refXML is not None:
            #print(etree.tostring(refXML,pretty_print=True))
            self.Include = refXML.get('Include').split(',')[0].strip()
            self.Private = refXML.find(MSBNS+'Private')
            if self.Private is not None:
                self.Private = self.Private.text == 'True'
            self.HintPath = refXML.find(MSBNS+'HintPath')
            if self.HintPath is not None:
                self.HintPath = self.HintPath.text.strip()

class ProjectReference:
    '''
    <ProjectReference Include="..\XB1Interface\XB1Interface.csproj" Condition="('$(Configuration)' == 'Debug_XB1') Or ('$(Configuration)' == 'Release_XB1')">
      <Project>{b6068a9d-54a1-425a-9dfb-87fd9ec5e822}</Project>
      <Name>XB1Interface</Name>
    </ProjectReference>
    '''
    def __init__(self, refXML=None):
        self.Include=''
        self.Condition=''
        self.Project=''
        self.Name=''
        if refXML is not None:
            self.Include = refXML.get('Include')
            self.Condition = refXML.get('Condition','')
            self.Project = refXML.find(MSBNS+'Project').text
            self.Name = refXML.find(MSBNS+'Name').text





class VS2015Project:

    def __init__(self):
        self.project = None
        self.references = {}
        self.projectrefs = {}
        self.reference_group = None
        self.projectref_group = None

        self.NSMAP = {None: 'http://schemas.microsoft.com/developer/msbuild/2003'}
        self.XPATH_NSMAP = {'x': 'http://schemas.microsoft.com/developer/msbuild/2003'}

    def LoadFromFile(self, filename):
        parser = etree.XMLParser(remove_blank_text=True)
        self.project = etree.parse(filename, parser)
        self.references = {}
        self.projectrefs = {}
        for reference in self.project.xpath('//x:Reference', namespaces=self.XPATH_NSMAP):
            refID = reference.get('Include').split(',')[0]
            self.references[refID] = reference
            self.reference_group = reference.getparent()
        for reference in self.project.xpath('//x:ProjectReference', namespaces=self.XPATH_NSMAP):
            refID = reference.find(MSBNS + 'Name').text
            self.projectrefs[refID] = reference
            self.projectref_group = reference.getparent()

    def SaveToFile(self, filename):
        with open(filename, 'w') as f:
            f.write(REG_FIND_BAD_SELFCLOSERS.sub(FIX_BAD_SELFCLOSERS, etree.tostring(self.project, pretty_print=True, xml_declaration=True)))

    def subelement(self, parent, name):
        return etree.SubElement(parent, MSBNS + name)

    def HasReference(self, refid):
        return refid in self.references or refid in self.projectrefs

    def AddFileRef(self, refID, hintpath, **kwargs):
        if kwargs.get('verbose', False):
            logging.info('Adding file reference %s.', refID)
        if 'verbose' in kwargs:
            del kwargs['verbose']
        reference = self.subelement(self.reference_group, 'Reference')
        reference.attrib['Include'] = refID

        kwargs['HintPath'] = hintpath

        for elementName, elementValue in kwargs.iteritems():
            self.subelement(reference, elementName).text = str(elementValue)

    def GetReference(self,refID):
        tag = self.references[refID]
        #print(tag.tag)
        if tag.tag == MSBNS+'Reference':
            return Reference(tag)
        else:
            return ProjectReference(tag)

    def RemoveRef(self, refID, verbose=False):
        if refID in self.references:
            if verbose:
                logging.info('Removed file reference %s.', self.references[refID].get('Include'))
            self.reference_group.remove(self.references[refID])
            del self.references[refID]
        if refID in self.projectrefs:
            if verbose:
                logging.info('Removed project reference %s.', self.projectrefs[refID].find(MSBNS + 'Name').text)
            self.projectref_group.remove(self.projectrefs[refID])
            del self.projectrefs[refID]


AddRefs = []
if __name__ == '__main__':
    logging.basicConfig(
        format='%(asctime)s [%(levelname)-8s]: %(message)s',
        datefmt='%m/%d/%Y %I:%M:%S %p',
        level=logging.DEBUG
    )

    if len(sys.argv) < 2 or not os.path.isdir(sys.argv[1]):
        logging.error('%s is not a directory.', sys.argv[1])
        logging.info('USAGE: python prep.py <path\\to\\SpaceEngineers>')
        sys.exit(1)

    config = {}
    if not os.path.isfile('user-config.yml'):
        data={
            'SECE': {
                'Builder': {
                    'Email': 'anonymous@change.me',
                    'Name': 'Change Me'
                },
                'Paths': {
                    'STOCK_BIN_DIR': sys.argv[1]
                }
            }
        }
        with open('user-config.yml','w') as f:
            yaml.dump(data,f)
    with open('config.yml', 'r') as f:
        config = yaml.load(f)
    newrefs = config.get('reference-fixes', [])
    for project_name in os.listdir('Sources'):
        logging.info('Fixing %s...', project_name)
        project_dir = os.path.join('Sources', project_name)
        for filename in os.listdir(project_dir):
            filename = os.path.join(project_dir, filename)
            if filename.endswith('.csproj'):
                if os.path.isfile(filename + '.user'):
                    os.remove(filename + '.user')
                if os.path.isfile(filename + '.new'):
                    os.remove(filename + '.new')
                userf = os.path.join(project_dir, project_name + '.user')
                if os.path.isfile(userf):
                    os.remove(userf)

                project = VS2015Project()
                project.LoadFromFile(filename)
                for refID in newrefs:
                    if project.HasReference(refID):
                        refInfo  = project.GetReference(refID)
                        newHintPath=os.path.join(SE_ROOT_DIR, 'Bin64', refID + '.dll')
                        if isinstance(refInfo, Reference) and refInfo.HintPath == newHintPath: continue
                        project.RemoveRef(refID, verbose=True)
                        project.AddFileRef(refID, newHintPath, verbose=True)
                project.SaveToFile(filename)
                break
