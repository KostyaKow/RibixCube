using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
   public string defaultColor;
   private Dictionary<string, Color> colors;
   private int counter;
   public List<GameObject> cubes;
   private float left_to_rotate;
   public float rot_frame_angle;

   public Vector3 align_helper;

	// Update is called once per frame
	void Update () {
      counter++;
      if (counter % 300 == 0 || counter == 1) {
         left_to_rotate += 90;
         //rotateStrip(counter % 3, true, true, 90);

         //var t = cubes[1].transform;
         //t.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
      }

      if (left_to_rotate > 0) {
         left_to_rotate -= rot_frame_angle;
         rotateStrip(0, false, true, rot_frame_angle);
      }
	}

   //List<GameObject> getFace(string s) {} //BFUDLR
   //bool isDone() {}

   //rotate row/column at index, vertically/horizontally, forward/backward
   void rotateStrip(int index, bool vert, bool forward, float angle) {
      //number of subCubes to rotate at a time: 9 (8 without subcube in middle)
      List<GameObject> cubes_to_rotate = new List<GameObject>();

      for (int x = 0; x < 3; x++) {
         for (int y = 0; y < 3; y++) {
            for (int z = 0; z < 3; z++) {
               if ((vert && index == x) || (!vert && index == y))
                  cubes_to_rotate.Add(cubes[x*3*3+y*3+z]);
            }
         }
      }

      float degrees = forward ? angle : -angle;

      Transform centralCubeT = cubes[1*3*3 + 1*3 + 1].transform;
      //move central square up to check if we got right now
      //if (counter == 1) centralCubeT.transform.position += new Vector3(0, 3, 0);

      //(0, 0, 0); //(0.5f, 0.5f, 0.5f);
      Vector3 alignV = align_helper;//new Vector3(-0.5f, 0, 1)/2 + new Vector3(0.29f, 0.1f, 0.075f);
      Vector3 rotatePoint = centralCubeT.position - alignV;

      if (vert) {
         for (int i = 0; i < cubes_to_rotate.Count; i++) {
            var t = cubes_to_rotate[i].transform;
            t.RotateAround(rotatePoint, Vector3.right, degrees);
         }
      } else {
         for (int i = 0; i < cubes_to_rotate.Count; i++) {
            var t = cubes_to_rotate[i].transform;
            t.RotateAround(rotatePoint, Vector3.up, degrees);
         }
      }
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
      counter = 0;
      left_to_rotate = 0.0f;

      cubes = new List<GameObject>();

      //int num_cubes = 3*3*3; //1 of them in middle which is never visible

      for (int i = 0; i < 3; i++) { //(x, y, z) = (i, j, k)
         for (int j = 0; j < 3; j++) {
            for (int k = 0; k < 3; k++) {
               var sub = makeSubCube();
               sub.name += i.ToString() + "-" +
                           j.ToString() + "-" +
                           k.ToString();

               var moveV = new Vector3(i+0.05f*i, j+0.05f*j, k+0.05f*k);
               sub.transform.position += moveV;
               cubes.Add(sub);
               if (k == 0) {
                  if (!setSubCubeSideColor(sub, "F", "green")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }
               if (k == 2) {
                  if (!setSubCubeSideColor(sub, "B", "yellow")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }
               if (i == 2) {
                  if (!setSubCubeSideColor(sub, "R", "orange")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }
               if (i == 0) {
                  if (!setSubCubeSideColor(sub, "L", "red")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }
               if (j == 0) {
                  if (!setSubCubeSideColor(sub, "D", "blue")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }
               if (j == 2) {
                  if (!setSubCubeSideColor(sub, "U", "white")) {
                     Debug.Log("can't initialize cube"); return;
                  }
               }

            }
         }
      }

	}

   GameObject makeQuadFace(string c_str = null, GameObject parent = null) {
      var fType = PrimitiveType.Quad;
      var face = GameObject.CreatePrimitive(fType);
      face.name = "SubCubeFace";

      if (c_str == null)
         c_str = defaultColor;

      //Debug.Log(c_str);
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
      o.name = "SubCubeContainer";

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

      setParent(o, gameObject);
      return o;
   }

   void initColors() {
      colors = new Dictionary<string, Color>();
      colors.Add("yellow", new Color(1, 1, 0));
      colors.Add("orange", new Color(1, 0.8f, 0));
      colors.Add("red", new Color(1, 0, 0));
      colors.Add("green", new Color(0, 1, 0));
      colors.Add("blue", new Color(0, 0, 1));
      colors.Add("white", new Color(1, 1, 1));

      colors.Add("black", new Color(0, 0, 0));
   }

   bool setSubCubeSideColor(GameObject o, string side_str, string color_str = null)
   {
      //var side = o.Find(side_str);
      var side = GameObject.Find(o.name + "/" + side_str);
      if (side == null) return false;
      Debug.Log("got side");
      if (color_str == null) color_str = "black";

      var c = colors[color_str];
      if (c == null) return false;

      setColor(side, c);
      return true;
   }

   void setColor(GameObject o, Color c) {
      Renderer rend = o.GetComponent<Renderer>();
      rend.material.color = c;
   }

   void setParent(GameObject child, GameObject parent) {
      child.transform.parent = parent.transform;
   }

}
