using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{

  public Rigidbody player;
  public GameObject scenario;
  public float speedScenario;
  private int currentLane;

  public float laneDistance;
  private Vector3 target;
  private Vector2 initalPosition;

  public GameObject floor;
  public GameObject scene_floor;
  public GameObject obstacle;
  public GameObject coin;

  public GameObject diamond;
  public GameObject spring;

  private int pontos = 0;
  private int ammo = 0;
  public Text txtPontos;
  public Text txtAmmo;

  private int currentStage = 1;

  public AudioClip coinsound;
  public AudioSource coin_catch;


  public AudioClip destroyed;
  public AudioSource playerFail;


  public AudioClip diamondRush;
  public AudioSource DiaRush;

  public AudioClip backgroundSound;
  public AudioSource backsound;


  private bool diamanteAtivo;
  private bool pular;



           public Vector3 jump;
         public float jumpForce = 2.0f;
     
         public bool isGrounded;
         Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

             rb = GetComponent<Rigidbody>();
             jump = new Vector3(0.0f, 2.0f, 0.0f);
      txtPontos.text = ""+pontos;
      txtAmmo.text = ""+ammo;
     currentLane = 1;   
     target = player.transform.position;
        buildScenario();


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
      if(player.transform.position.y > 0.4){
        
        player.transform.position = Vector3.Lerp(player.transform.position, target, 5*Time.deltaTime);
      target.y = 0.5F;
      }

      if(pular){
        target.y = 25;
        player.transform.position = Vector3.Lerp(player.transform.position, target, 5*Time.deltaTime);

                //  rb.AddForce(jump * jumpForce * 10, ForceMode.Impulse);
                //  isGrounded = false;
       pular = false;

      }
  

      scenario.transform.Translate(0,0, speedScenario * Time.deltaTime * -1);

        float scenarioZ = scenario.transform.position.z;
        int stage = (int)(Mathf.Floor((scenarioZ - 80.0F) / -100.0F) + 1);
        if(stage > currentStage)        {
            GameObject floor2 = Instantiate(floor);
            GameObject scene_floor2 = Instantiate(scene_floor);
            float posz = ((100 * currentStage) + 15) + scenarioZ;
            float posx = floor.transform.position.x;
            float posy = floor.transform.position.y;

            floor2.transform.SetParent(scenario.transform);
            floor2.transform.position = new Vector3(posx, posy, posz);

            posx = scene_floor.transform.position.x;
            posy = scene_floor.transform.position.y;

            scene_floor2.transform.SetParent(scenario.transform);
            scene_floor2.transform.position = new Vector3(posx, posy, posz);

            currentStage++;

            buildScenario();
        }
    }

    void buildScenario(){
        int start = 0;
        int verifier =0;
        if (currentStage == 1){
            start = 2;
        }

        for (int i=start; i < 7; i++){
            int[] element = new int[3];

            for (int j=0; j < 3; j++){
                element[j] = Random.Range(0, 3);

                if (element[0] == 1 && element[1] == 1 && element[2] == 1){
                    element[2] = 0;
                }


                if(diamanteAtivo == false){
                  if (element[j] == 1){
                      instantiateObstacle(i, j);
                  }
                  else if (element[j] == 2){
                      instantiateCoin(i, j);
                      
                  }
                }
                else{
                  instantiateCoin(i, j);


                  if(verifier == 1){
                    backsound.Stop();

                    DiaRush.clip = diamondRush;
                    DiaRush.Play ();
                  }
                  if(verifier > 10){
                    diamanteAtivo = false;
                    DiaRush.Stop();
                    
                    backsound.clip = backgroundSound;
                    backsound.Play ();
                  }

                  verifier++;
                }

            }
        }
    }

    void instantiateObstacle(int indexZ, int indexX){
        GameObject obstacle2 = Instantiate(obstacle);
        float posZ = ((14.28F * indexZ)+ ((currentStage - 1) * 100) + scenario.transform.position.z) - 20;
        float posX = (indexX - 1) * laneDistance;
        obstacle2.transform.SetParent(scenario.transform);
        obstacle2.transform.position = new Vector3(posX, 0.5F, posZ);
    }

    void instantiateCoin(int indexZ, int indexX)
    {
        GameObject coin2 = Instantiate(coin);
        float posZ = ((14.28F * indexZ) + ((currentStage - 1) * 100) + scenario.transform.position.z) - 20;
        float posX = (indexX - 1) * laneDistance;
        coin2.transform.SetParent(scenario.transform);
        coin2.transform.position = new Vector3(posX, 0.5F, posZ);
    }
  




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            pontos = pontos + 10;
            ammo++;
      txtPontos.text = ""+pontos;
      txtAmmo.text = ""+ammo;

              coin_catch.clip = coinsound;
              coin_catch.Play ();
        }
        if (other.gameObject.CompareTag("obstacle"))
        {
              playerFail.clip = destroyed;
              playerFail.Play ();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        if (other.gameObject.CompareTag("diamond"))
        {
            Destroy(other.gameObject);
            pontos = pontos * 2;
            txtPontos.text = ""+pontos;
            diamanteAtivo = true;

        }
        if (other.gameObject.CompareTag("spring"))
        {
          Destroy(other.gameObject);
            pontos = pontos + 50 ;
            txtPontos.text = ""+pontos;
            pular = true;

        }
    }
}