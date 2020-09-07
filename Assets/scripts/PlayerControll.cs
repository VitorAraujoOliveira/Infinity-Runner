using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviour
{

  public Rigidbody player;
  public GameObject scenario;
  public float speedScenario;
  private int currentLane;

  public float laneDistance;
  private Vector3 target;
  private Vector2 initalPosition;


    // Start is called before the first frame update
    void Start()
    {
     currentLane = 1;   
     target = player.transform.position;
    }

    // Update is called once per frame
    void Update() {
   

      int newLane = -1;
      // keyboard
      if(Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2) {
        newLane = currentLane + 1;
      } else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0) {
        newLane = currentLane - 1;
      }
      
      // mouse
      if(Input.GetMouseButtonDown(0)) {
        initalPosition = Input.mousePosition;
      } else if (Input.GetMouseButtonUp(0)) {
        if (Input.mousePosition.x > initalPosition.x && currentLane < 2) {
          newLane = currentLane + 1;
        } else if (Input.mousePosition.x < initalPosition.x && currentLane > 0) {
          newLane = currentLane - 1;
        }
      }

      // Touch
      if (Input.touchCount >= 1) {
        if (Input.GetTouch(0).phase == TouchPhase.Began) {
          initalPosition = Input.GetTouch(0).position;
        } else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
          if (Input.GetTouch(0).position.x > initalPosition.x && currentLane < 2) {
            newLane = currentLane + 1;    
          } else if (Input.GetTouch(0).position.x < initalPosition.x && currentLane > 0) {
            newLane = currentLane - 1;
          }
        }
      } 
      if (newLane>= 0){
        currentLane = newLane;
        target = new Vector3((currentLane - 1) * laneDistance, player.transform.position.y, player.transform.position.z);
      }
      if(player.transform.position.x != target.x){
        player.transform.position = Vector3.Lerp(player.transform.position, target, 5*Time.deltaTime);
      }

      scenario.transform.Translate(0,0, speedScenario * Time.deltaTime * -1);
    }

    /*
    void OnCollisionEnter(Collision col) {
        Debug.Log(col.gameObject.tag);
      if (col.gameObject.CompareTag("coin")) {
        Destroy(col.gameObject);
      } 
      if (col.gameObject.CompareTag("obstacle")) {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
      }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("obstacle"))
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
