using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skybox_color : MonoBehaviour
{
    public string animClipName;
    private Animation anim;
    public Color currentColor;
    public Color finalColor;
    public Material skybox_mat;
    public Material expansion_mat;
    private Color tmpColor;
    private float hueShift;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        StartCoroutine(ColorUpdate());
    }

    public IEnumerator ColorUpdate() 
    {
        currentColor = skybox_mat.GetColor("_Tint");
        finalColor = expansion_mat.GetColor("_Color");
        anim.Play(animClipName);
		yield return WaitForAnim(animClipName);
    }

    IEnumerator WaitForAnim(string animClipName)
    {
		
        yield return null;
        //Debug.Log(tempTime);
        while (anim.IsPlaying(animClipName)){

            // convert from RGB to HSV
            Color.RGBToHSV(currentColor, out float hue, out float sat, out float val);
            Color.RGBToHSV(finalColor, out float hue2, out float sat2, out float val2);

            hueShift = Mathf.Lerp(hueShift, hue, hue2);
            skybox_mat.SetColor("_Tint", Color.HSVToRGB(hueShift, sat2, val2));
            yield return null;

        }
    }
}
