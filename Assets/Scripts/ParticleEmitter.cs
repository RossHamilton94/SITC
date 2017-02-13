using UnityEngine;
using System.Collections;

public class ParticleEmitter : MonoBehaviour {
    //Variable to get the ppositioning of the boss to trigger the particle effect.
    GameObject boss;
    //Reference to the explosion.
    GameObject[] explosion;
    //Reference to the smoke.
    GameObject smoke;

    void Start()
    {
        boss = this.gameObject;
        explosion = GameObject.FindGameObjectsWithTag("Explosion");
        smoke = GameObject.Find("Smoke");
    }

    void Update()
    {
        if (boss != null)
        {
            //Debug.Log("Component has been attached successfully at: " + this.gameObject.transform.position.ToString());
            Debug.Log("Boss is present in the scene.");
        }
        else
        {
            Debug.LogError("The object was not attached");
        }
    }
    #region Commented out methods
    /**public void PopulateExplosionEmitter(GameObject burstLocation)
    {
        //Populate emitters with each one coming from the same point- around the object.
        emitter1 = new ParticleSystem();
        emitter2 = new ParticleSystem();
        emitter3 = new ParticleSystem();
        emitter4 = new ParticleSystem();
        emitter5 = new ParticleSystem();
        emitter6 = new ParticleSystem();

        emitter1.transform.position = burstLocation.transform.position;
        emitter2.transform.position = burstLocation.transform.position;
        emitter3.transform.position = burstLocation.transform.position;
        emitter4.transform.position = burstLocation.transform.position;
        emitter5.transform.position = burstLocation.transform.position;
        emitter6.transform.position = burstLocation.transform.position;

        //Once that has been done then rotate the emitters to be facing away from the object and finally emit bursts.
        /**Downwards emitter2.transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
        /**Left emitter3.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
        /**Right emitter4.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        /**Up emitter5.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

        //Finally is the backwards burst.
        emitter6.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

        emitter1.startSpeed = 10.0f;
        emitter2.startSpeed = 10.0f;
        emitter3.startSpeed = 10.0f;
        emitter4.startSpeed = 10.0f;
        emitter5.startSpeed = 10.0f;
        emitter6.startSpeed = 10.0f;

        RunEmitter(1.0f);
    }

    public void RunEmitter(float duration)
    {
        emitter1.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);
        emitter2.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);
        emitter3.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);
        emitter4.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);
        emitter5.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);
        emitter6.startColor = Color.Lerp(Color.white, Color.red, Time.time / duration);

        emitter1.Emit(50);
        emitter2.Emit(50);
        emitter3.Emit(50);
        emitter4.Emit(50);
        emitter5.Emit(50);
        emitter6.Emit(50);
    }
*/
    #endregion

    public void CreateExplosion(Vector3 hitPos)
    {
        explosion[0].transform.position = hitPos;
        explosion[0].GetComponent<ParticleSystem>().Play();

        for (int i = 1; i < 5; i++)
        {
            Instantiate(explosion[0], (hitPos * Random.Range(1, 2)), Quaternion.identity);
            explosion[i].GetComponent<ParticleSystem>().Play();
            Destroy(explosion[i].gameObject);
        }

        //Debug.Log("The explosion worked");
        //Instantiate(explosion, hitPos, Quaternion.identity);
    }
}
