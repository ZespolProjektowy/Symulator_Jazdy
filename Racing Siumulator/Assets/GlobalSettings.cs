using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public enum Controls
    {
        playerControls,
        aIControls,
        bPAIControls
    };

    [Space(20)]
    public Controls whichControls = Controls.playerControls;
    [Space(20)]

    [HideInInspector]
    public bool usePlayerControls = false;
    [HideInInspector]
    public bool useAiControls = true;
    [HideInInspector]
    public bool useBPControls = false;

    // Start is called before the first frame update
    void Start()
    {

        if (whichControls == Controls.playerControls)
        {
            usePlayerControls = true;
            useAiControls = false;
            useBPControls = false;
        }
        else if (whichControls == Controls.aIControls)
        {
            usePlayerControls = false;
            useAiControls = true;
            useBPControls = false;
        }
        else if (whichControls == Controls.bPAIControls)
        {
            usePlayerControls = false;
            useAiControls = false;
            useBPControls = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
