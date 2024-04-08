using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [SerializeField] public TMP_Text debugText;
    [SerializeField] public TMP_Text timerText;
    [SerializeField] public Button timerActivation;

    private string filepath = "";
    private float timer = 10;
    private List<float> velocities = new List<float>();
    private bool canRun = false;

    // Start is called before the first frame update
    void Start()
    {
        timerActivation.onClick.AddListener(StartTimer);

        filepath = Application.persistentDataPath + "/acceleration.csv";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canRun) { return; }
        debugText.text = Input.acceleration.magnitude.ToString();

        velocities.Add(Input.acceleration.magnitude);

    }

    public void StartTimer()
    {
        timerActivation.interactable = false;
        canRun = true;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString();
            yield return null;
        }

        timerText.text = "0.00";
        SerializeCSV();
    }

    public void SerializeCSV()
    {
        StreamWriter sw = new StreamWriter(filepath);

        sw.WriteLine("Velocity");

        for (int i = 0; i < velocities.Count; i++)
        { 
            sw.WriteLine(velocities[i]);

        }

        sw.Flush();
        sw.Close();
    }
}
