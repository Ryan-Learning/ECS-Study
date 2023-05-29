using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    ITankForce tankf;
    public ICustomKey keys;
    ITankShoot bulletController;

    public float runSpeed;
    public float rotateSpeed;
    public float bulletSpeed;
    public GameObject bullet;
    public Transform barrel_Target;

    public void Awake()
    {
        tankf = new TankForce(CustomKey.GetInstance(), transform);
        keys = CustomKey.GetInstance();
        bulletController = new TankShoot(transform, bullet, barrel_Target);
    }

    void Update()
    {
        if (!tankf.isCollision) {
            tankf.DoMovement();
        }
        else {
            tankf.DoPrev();
        }
        if (keys.DoShoot()) {
            bulletController.Shoot();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WallArea") tankf.isCollision = true;
    }

    public void OnDrawGizmos()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * 10f;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(startPosition, endPosition);
    }
}


public interface ICustomKey
{
    Hashtable GetCode();
    bool DoShoot();
}

public class CustomKey : ICustomKey
{
    Hashtable keys = new Hashtable();
    static CustomKey self;

    public static CustomKey GetInstance()
    {
        return self ??= new CustomKey();
    }

    private CustomKey() { }


    public Hashtable GetCode()
    {
        keys["W"] = Input.GetKey(KeyCode.W);
        keys["S"] = Input.GetKey(KeyCode.S);
        keys["A"] = Input.GetKey(KeyCode.A);
        keys["D"] = Input.GetKey(KeyCode.D);

        return keys;
    }

    public bool DoShoot()
    {
        return Input.GetKey(KeyCode.Mouse0);
    }
}


public interface ITankForce
{
    void DoMovement();
    void DoPrev();
    bool isCollision { get; set; }
}


public class TankForce : ITankForce
{
    public ICustomKey keys;
    public bool isCollision { get; set; }
    private TankMovement tankMovement;
    private Transform tf;
    private Vector3 prev;


    public TankForce(ICustomKey keys, Transform tf)
    {
        this.keys = keys;
        this.tf = tf;
        tankMovement = tf.GetComponent<TankMovement>();
    }

    public void DoMovement()
    {
        Hashtable code = keys.GetCode();
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float runSpeed = tankMovement.runSpeed;
        float rotateSpeed = tankMovement.rotateSpeed;
        float delta = Time.deltaTime;

        if (Check("W", "S")) {
            float positionAmount = moveY * runSpeed * delta;
            prev = tf.localPosition;
            tf.localPosition += tf.TransformDirection(Vector3.forward) * positionAmount;
        }
        if (Check("A", "D")) {
            float rotationAmount = moveX * rotateSpeed * delta;
            tf.Rotate(Vector3.up, rotationAmount);
        }
    }

    public void DoPrev()
    {
        tf.localPosition = prev;
        isCollision = false;
    }

    bool Check(params string[] pars)
    {
        Hashtable code = keys.GetCode();
        bool ans = false;
        foreach (var k in pars) {
            ans = ans || (bool)keys.GetCode()[k];
        }
        return ans;
    }

}

public interface ITankShoot
{
    void Shoot();
}

public class TankShoot : ITankShoot
{
    GameObject bulletProtal;
    Transform target;
    Transform barrel_Target;
    Vector3 bulletForword;

    float bulletSpeed; // 子弹的速度

    public TankShoot(Transform target, GameObject bullet, Transform barrel_Target)
    {
        this.target = target;
        this.bulletProtal = bullet;
        this.barrel_Target = barrel_Target;
    }

    public void Shoot()
    {
        bulletForword = target.GetComponentInChildren<TankTurretController>().transform.forward;
        bulletSpeed = target.GetComponent<TankMovement>().bulletSpeed;
        GameObject bullet = Object.Instantiate(bulletProtal, barrel_Target.position, new Quaternion());
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity += bulletForword * bulletSpeed;
    }
}