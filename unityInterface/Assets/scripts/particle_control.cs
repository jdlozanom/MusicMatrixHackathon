using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_control : MonoBehaviour {

	public int channel = 0;
	public int audio_component = 3;
	public Material asset_mat;
	public float radiusMag = 1000f;
	public float emissionMag = 100f;
	public float startSizeMag = 10f;
	public float speedMultiplier = 50f;

	private float H, S, V, start_V, final_V;
	private Color startColor;
	private Color tmpColor;


	public ParticleSystem ps;
	public ParticleSystem rs;
	private ParticleSystem.ShapeModule pShape;
	private ParticleSystem.MainModule pMain;
	private ParticleSystem.VelocityOverLifetimeModule vMain;
	private ParticleSystem.MainModule rMain;


	// Use this for initialization
	void Start () {
		//ps = GetComponent<ParticleSystem>();
		pShape = ps.shape;
		pMain = ps.main;
		vMain = ps.velocityOverLifetime;

		rMain = rs.main;	

	}
	
	// Update is called once per frame
	void Update () {
		pShape.radius = Mathf.Abs(OSC_channels.OSCch_data[channel,audio_component]) * radiusMag;
		pShape.angle = Mathf.Abs(OSC_channels.OSCch_data[channel,audio_component]) * 10f;
		float tmpSize = OSC_channels.OSCch_data[channel,2] * startSizeMag;

		//color set and conversion to HSV
		startColor = asset_mat.GetColor("_EmissionColor");
		Color.RGBToHSV(startColor, out H, out S, out V);

		//emission adjustments
		start_V = Mathf.Abs(OSC_channels.OSCch_data[channel,2]) * 3f;

		//particle emission and size adjustments
		float tmp_emission = Mathf.Abs(OSC_channels.OSCch_data[channel,2]) * emissionMag;
		ps.emissionRate = tmp_emission;
		pMain.startSize = new ParticleSystem.MinMaxCurve(tmpSize + 2f, tmpSize + 5f);

		float tmp_speed = OSC_channels.OSCch_data[channel,2] * speedMultiplier;
		pMain.startSpeed = new ParticleSystem.MinMaxCurve(tmp_speed+12f, tmp_speed+15f);
		
	}
	void LateUpdate(){
		final_V = Mathf.LerpUnclamped(V, Mathf.Abs(OSC_channels.OSCch_data[channel,1]) * 5f, 0.3f * Time.deltaTime);

		asset_mat.SetColor("_EmissionColor", Color.HSVToRGB(H,S,final_V, true));
	}
}
