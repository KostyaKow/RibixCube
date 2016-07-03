using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
   public string defaultColor;
   private Dictionary<string, Color> colors;

   GameObject makeQuadFace(string c_str = null, GameObject parent = null) {
      var fType = PrimitiveType.Quad;
      var face = GameObject.CreatePrimitive(fType);
      face.name = "SubCubeFace";

      if (c_str == null)
         c_str = defaultColor;

      Debug.Log(c_str);
      Color c = colors[c_str]; //new Color(0, 0, 0);

      Renderer rend = face.GetComponent<Renderer>();

      //Color c2 = new Color(c.r + 100, c.b - 100, c.g + 150);
      //float duraction = 1.0f;
      rend.material.color = c;

      if (parent != null)
         setParent(face, parent);
      return face;
   }

   //color values: far, close, top, bottom, left, right
   //B, F, U, D, L, R
   GameObject makeSubCube(string c1 = null, string c2 = null,
                          string c3 = null, string c4 = null,
                          string c5 = null, string c6 = null)
   {

      GameObject o = new GameObject(); //Parent Object
      o.name = "SubCube Container";

      var face_far = makeQuadFace(c1, o);
      face_far.name = "B";
      face_far.transform.eulerAngles += new Vector3(0, 180, 0);

      var face_close = makeQuadFace(c2, o);
      face_close.name = "F";
      face_close.transform.position += new Vector3(0, 0, -1.0f);

      var face_top = makeQuadFace(c3, o);
      face_top.name = "U";
      var ftt = face_top.transform;
      ftt.eulerAngles += new Vector3(90, 0, 0);
      ftt.position += new Vector3(0, 0.5f, -0.5f);

      var face_bottom = makeQuadFace(c4, o);
      face_bottom.name = "D";
      var fbt = face_bottom.transform;
      fbt.eulerAngles += new Vector3(-90, 0, 0);
      fbt.position += new Vector3(0, -0.5f, -0.5f);

      var face_left = makeQuadFace(c5, o);
      face_left.name = "L";
      var flt = face_left.transform;
      flt.eulerAngles += new Vector3(0, 90, 0);
      flt.position += new Vector3(-0.5f, 0, -0.5f);

      var face_right = makeQuadFace(c6, o);
      face_right.name = "R";
      var frt = face_right.transform;
      frt.position += new Vector3(0.5f, 0, -0.5f);
      frt.eulerAngles += new Vector3(0, 90+180, 90);

      return o;
   }

	/* (F)ront = green
      (B)ack = yellow
      (L)eft = red
      (R)ight = orange
      (U)p = white
      (D)own = blue

      Start  with green (F)
   */
	void Start() {
      //defaultColor = "black"
      initColors();

      List<GameObject> cube = new List<GameObject>();

      int num_cubes = 3*3*3; //1 of them in middle which is never visible

      for (int i = 0; i < 3; i++) {
         for (int j = 0; j < 3; j++) {
            for (int k = 0; k < 3; k++) {
               var face = makeSubCube("black");
               face.transform.position += new Vector3(i+0.1f*i, j+0.1f*j, k+0.1f*k);
            }
         }
      }
	}

   Dictionary<string, string> getNeighbors(string color) {
      return new Dictionary<string, string>();
   }

	// Update is called once per frame
	void Update () {

	}

   void setParent(GameObject child, GameObject parent) {
      child.transform.parent = parent.transform;
   }

   void initColors() {
      colors = new Dictionary<string, Color>();
      colors.Add("red", new Color(1, 0, 0));
      colors.Add("green", new Color(0, 1, 0));
      colors.Add("blue", new Color(0, 0, 1));
      colors.Add("black", new Color(0, 0, 0));
      colors.Add("white", new Color(1, 1, 1));
   }

}
