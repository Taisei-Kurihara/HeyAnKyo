using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Respown : MonoBehaviour
{
    public Text gameOverText;  // �Q�[���I�[�o�[���̃e�L�X�g (Inspector �Őݒ�)
    public Text livesText;     // �c�@�\���e�L�X�g (Inspector �Őݒ�)
    public Text respawnText;   // �X�y�[�X�L�[�Ń��X�|�[���\�ł��邱�Ƃ������e�L�X�g (Inspector �Őݒ�)

    private int lives = 3; // �c�@��
    private Vector3 lastDeathPosition; // ���S���̈ʒu���L�^

    private void Start()
    {
        UpdateLivesText();
        respawnText.text = ""; // ������Ԃł͔�\��
    }

    private void OnTriggerEnter(Collider other)
    {
        // �G (�^�O: enemy) �ɓ��������ꍇ�̂ݏ���
        if (other.gameObject.CompareTag("enemy"))
        {
            lives--; // �c�@�����炷
            lastDeathPosition = transform.position; // ���S�ʒu���L�^
            UpdateLivesText(); // �c�@�� UI ���X�V

            if (lives <= 0)
            {
                gameOverText.text = "Game Over"; // �Q�[���I�[�o�[�\��
                respawnText.text = ""; // �Q�[���I�[�o�[���̓��X�|�[�����b�Z�[�W������
                Destroy(gameObject); // �v���C���[�I�u�W�F�N�g���폜
            }
            else
            {
                respawnText.text = "Press SPACE to Respawn"; // ���X�|�[���\�ł��邱�Ƃ�ʒm
            }
        }
    }

    private void Update()
    {
        // �X�y�[�X�L�[��������A�c�@���c���Ă���ꍇ���X�|�[��
        if (Input.GetKeyDown(KeyCode.Space) && lives > 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = lastDeathPosition; // �Ō�Ɏ��񂾈ʒu�Ń��X�|�[��
        respawnText.text = ""; // ���X�|�[����̓��b�Z�[�W��\��
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives; // �c�@���� UI �ɔ��f
    }
}
