using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller; //������Ʈ

    private Vector3 moveVector;                                 // ���� ����
    private float vertical_velocity = 0.0f;                     // ������ ���� ���� �ӵ�
    private float gravity = 12.0f;                              // �߷� ��
    
    [SerializeField] private float speed = 5.0f;                // �÷��̾��� �̵� �ӵ�
    [SerializeField] private float jump = 3.0f;                 // �÷��̾��� ���� ��ġ

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //ī�޶� ��Ʈ�ѷ��� �̿��� �÷��̾� ������ ���� ī�޶� ������ �����غ��� �մϴ�.
        if(Time.timeSinceLevelLoad < CameraController.camara_animate_duration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        moveVector = Vector3.zero; //���� ���� �� ����
        //���� ������� ��� velocity ����
        if(controller.isGrounded)
        {
            Debug.Log("��Ʈ�ѷ��� ���� ��ҽ��ϴ�.");




            vertical_velocity = -0.5f;

            //���� ��� �߰�
            if(Input.GetKeyDown(KeyCode.X))
            {
                vertical_velocity = jump;
            }
        }
        else
        {
            //�ƴ� ��� �߷�ġ��ŭ ����������
            vertical_velocity -= gravity * Time.deltaTime;
        }
        //1. �¿� �̵�
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed;
        //2. ���� ����
        moveVector.y = vertical_velocity;
        //3. ������ �̵�
        moveVector.z = speed;
        //������ ������ �̵� ���� 
        controller.Move(moveVector * Time.deltaTime);
    }
}
