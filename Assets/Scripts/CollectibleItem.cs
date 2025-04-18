using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public int scoreValue = 10;

    private GameManager gameManager;

    [Header("Audio")]
    [SerializeField] private AudioSource collectSFX; // AudioSource untuk efek suara

    private void Start()
    {
        // Cek jika collectSFX belum di-assign di Inspector
        if (collectSFX == null)
        {
            collectSFX = gameObject.AddComponent<AudioSource>();  // Menambahkan AudioSource jika belum ada
        }

        // Pastikan audio clip sudah terpasang di AudioSource
        if (collectSFX.clip == null)
        {
            Debug.LogError("Audio Clip not assigned to collectSFX!");
        }

        // Temukan GameManager dalam scene
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Deteksi tabrakan dengan pemain
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectSFX != null && collectSFX.clip != null)
            {
                collectSFX.PlayOneShot(collectSFX.clip);
            }
            else
            {
                Debug.LogError("AudioSource or Audio Clip not assigned!");
            }

            // Tambahkan skor ke GameManager
            gameManager.AddScore(scoreValue);

            // Hapus item setelah dikumpulkan
            Destroy(gameObject);
        }
    }
}
