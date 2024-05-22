using UnityEngine;

public class Recoil : MonoBehaviour
{

    //Rotations
    private Vector3 currentRotation; //Vector representing current rotation applied to weapon
    private Vector3 targetRotation; //Vector representing the target rotation

    bool isAiming;

    //Hip fire Recoil
    [SerializeField] private float recoilX; //Recoil values for hipfiring in X
    [SerializeField] private float recoilY; //Recoil values for hipfiring in Y
    [SerializeField] private float recoilZ; //Recoil values for hipfiring in Z

	//ADS Recoil
	[SerializeField] private float aimRecoilX; //Recoil values for ads in X
	[SerializeField] private float aimRecoilY; //Recoil values for ADS in Y
	[SerializeField] private float aimRecoilZ; //Recoil values for ADS in Z

	//Settings
	[SerializeField] private float snappiness; //How fast the recoil applies
    [SerializeField] private float returnSpeed; //How quickly recoil returns to normal aim 

    // Update is called once per frame
    void Update()
    {
        //Checking if we're ADS
        isAiming = AimScript.IsAiming;
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero,returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation,targetRotation,snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// Method to handle adding recoil per shot
    /// </summary>
    public void RecoilFire()
    {
        //When ADS enter here
        if (isAiming)
        {
            //Sets rotation according to parameterse
            targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
        }
        //When hipfiring enter here
        else
        {
            //Sets rotation according to parameters
			targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
		}
        
    }
}
