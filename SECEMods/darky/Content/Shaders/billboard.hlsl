// This multiplier makes reflective surfaces more transparent and thus reduced the glow they emit even in darkness.
// Setting it to 1.0f will result in vanilla behavior while 0.0f will make them completely transparent.
#define REFLECTIVE_COLOR_MULTI 0.05f

// This multiplier changes the specular reflection color (mirrored objects) on reflective surfaces like glass.
// Setting it to 0.95 will result in vanilla behavior while 0.0f will disable it completely.
//
// Note: As the REFLECTIVE_COLOR_MULTI is only 1/19th the original value, the reflections casted onto the surface are increased by the factor 3 by default.
//       This is to have a specular effect on glass even though the overall texture got reduced drastically.
#define REFLECTIVE_SPECULAR_GLOSS 3.0f

#include <Math/Color.h>
#include <csm.h>
#include <EnvAmbient.h>
#include <Transparency/Globals.h>
#include <billboard.h>

Texture2D<float4> TextureAtlas : register( t0 );
Texture2D<float> Depth : register( t1 );

#define REFLECTIVE

struct VsOut
{
    float4 position : SV_Position;
    float2 texcoord : Texcoord0;
    uint   index : Texcoord1;
    float3 wposition : TEXCOORD2;
#if defined(LIT_PARTICLE)
    float3 light : Texcoord3;
#endif
};

VsOut __vertex_shader(VsIn vertex, uint vertex_id : SV_VertexID)
{
    float4 projPos;
    uint billboard_index;
    CalculateVertexPosition(vertex, vertex_id, projPos, billboard_index);

    VsOut result;
    result.position = projPos;
    result.texcoord = vertex.texcoord;
    result.index = billboard_index;
    result.wposition = vertex.position.xyz;
#ifdef LIT_PARTICLE
    //float3 vs_pos = mul(float4(vertex.position.xyz, 1), frame_.view_matrix).xyz;
    float3 V = normalize(get_camera_position() - vertex.position.xyz);
    result.light = calculate_shadow_fast_particle(vertex.position.xyz, -result.position.z / result.position.w) + ambient_diffuse(V, 0.5f);
#endif

    return result;
}

#pragma warning( disable : 3571 )

float4 SaturateAlpha(float4 resultColor, float alpha, float alphaSaturation)
{
    if ( alphaSaturation < 1 )
    {
        float invSat = 1 - alphaSaturation;
        float alphaSaturate = clamp(alpha - invSat, 0, 1);
        resultColor += float4(1, 1, 1, 1) * float4(alphaSaturate.xxx, 0) * alpha;
    }
    return resultColor;
}

float4 CalculateColor(VsOut input, float particleDepth, bool minTexture, float alphaCutout)
{
    float depth = Depth[input.position.xy].r;
    float targetDepth = linearize_depth(depth, frame_.projection_matrix);
    float softParticleFade = CalcSoftParticle(BillboardBuffer[input.index].SoftParticleDistanceScale, targetDepth, particleDepth);

    float4 billboardColor = float4(BillboardBuffer[input.index].Color.xyz, BillboardBuffer[input.index].Color.w);

    float4 resultColor = float4(1, 1, 1, 1);

    if ( minTexture )
    {
        resultColor *= billboardColor;
        resultColor *= softParticleFade;
    }
    else
    {
        float4 textureSample = TextureAtlas.Sample(LinearSampler, input.texcoord.xy);
        //float alpha = textureSample.x * textureSample.y * textureSample.z;

        resultColor *= textureSample * billboardColor;
        //resultColor += 100*BillboardBuffer[input.index].Emissivity*resultColor; TODO
        //resultColor = SaturateAlpha(resultColor, alpha, BillboardBuffer[input.index].AlphaSaturation); uncomment for hotspots on lights/thruster flames

#ifdef ALPHA_CUTOUT
        float cutout = step(alphaCutout, resultColor.w);
        resultColor = float4(cutout * resultColor.xyz, cutout);
        //resultColor = float4(resultColor.w, resultColor.w, resultColor.w, 1);
#endif

        resultColor *= softParticleFade;
    }
    return resultColor;
}

void __pixel_shader(VsOut vertex, out float4 accumTarget : SV_TARGET0, out float4 coverageTarget : SV_TARGET1)
{
    float4 resultColor = float4(1, 1, 1, 1);

#ifdef ALPHA_CUTOUT
    float alphaCutout = BillboardBuffer[vertex.index].AlphaCutout;
#else
    float alphaCutout = 0;
#endif

    float linearDepth = linearize_depth(vertex.position.z, frame_.projection_matrix);

#ifdef REFLECTIVE
    float reflective = BillboardBuffer[vertex.index].reflective;
    if ( reflective )
    {
        float3 N = normalize(BillboardBuffer[vertex.index].normal);
        float3 viewVector = normalize(get_camera_position() - vertex.wposition);

        float3 reflectionSample = ambient_specular(0.04f, REFLECTIVE_SPECULAR_GLOSS, N, viewVector);
        float4 color = CalculateColor(vertex, linearDepth, true, alphaCutout);
        color.xyz *= color.w;
        float3 reflectionColor = lerp(color.xyz*color.w, reflectionSample, reflective);

        float4 dirtSample = TextureAtlas.Sample(LinearSampler, vertex.texcoord.xy);
        float3 colorAndDirt = lerp(reflectionColor, dirtSample.xyz, dirtSample.w);

        resultColor = REFLECTIVE_COLOR_MULTI * float4(colorAndDirt, max(max(color.w, reflective), dirtSample.w));
    }
    else
#endif
    {
        resultColor = CalculateColor(vertex, linearDepth, false, alphaCutout);
#ifdef LIT_PARTICLE
        resultColor.xyz *= vertex.light;
#endif
    }

    TransparentColorOutput(resultColor, linearDepth, vertex.position.z, 1.0f, accumTarget, coverageTarget);
}
