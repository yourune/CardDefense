using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text waveStateText;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private TMP_Text buttonText;
    
    [Header("Optional")]
    [SerializeField] private GameObject bossWaveIndicator;
    
    private void OnEnable()
    {
        if (WaveSystem.Instance != null)
        {
            WaveSystem.Instance.OnWaveStart += HandleWaveStart;
            WaveSystem.Instance.OnWaveComplete += HandleWaveComplete;
            WaveSystem.Instance.OnAllWavesComplete += HandleAllWavesComplete;
            WaveSystem.Instance.OnWaveCountChanged += HandleWaveCountChanged;
        }
        
        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);
        }
    }
    
    private void OnDisable()
    {
        if (WaveSystem.Instance != null)
        {
            WaveSystem.Instance.OnWaveStart -= HandleWaveStart;
            WaveSystem.Instance.OnWaveComplete -= HandleWaveComplete;
            WaveSystem.Instance.OnAllWavesComplete -= HandleAllWavesComplete;
            WaveSystem.Instance.OnWaveCountChanged -= HandleWaveCountChanged;
        }
        
        if (startWaveButton != null)
        {
            startWaveButton.onClick.RemoveListener(OnStartWaveButtonClicked);
        }
    }
    
    private void Update()
    {
        UpdateEnemyCount();
        UpdateButtonState();
    }
    
    private void HandleWaveStart(int waveNumber)
    {
        if (waveStateText != null)
        {
            WaveData currentWave = WaveSystem.Instance.CurrentWave;
            string stateText = currentWave != null && currentWave.isBossWave 
                ? "BOSS WAVE!" 
                : "Wave In Progress";
            waveStateText.text = stateText;
        }
        
        if (bossWaveIndicator != null && WaveSystem.Instance.CurrentWave != null)
        {
            bossWaveIndicator.SetActive(WaveSystem.Instance.CurrentWave.isBossWave);
        }
    }
    
    private void HandleWaveComplete(int waveNumber, WaveData waveData)
    {
        if (waveStateText != null)
        {
            waveStateText.text = "Wave Cleared!";
        }
        
        if (bossWaveIndicator != null)
        {
            bossWaveIndicator.SetActive(false);
        }
    }
    
    private void HandleAllWavesComplete()
    {
        if (waveStateText != null)
        {
            waveStateText.text = "VICTORY!";
        }
        
        if (startWaveButton != null)
        {
            startWaveButton.gameObject.SetActive(false);
        }
    }
    
    private void HandleWaveCountChanged(int currentWave, int totalWaves)
    {
        if (waveNumberText != null)
        {
            waveNumberText.text = $"Wave {currentWave}/{totalWaves}";
        }
    }
    
    private void UpdateEnemyCount()
    {
        if (enemyCountText != null && WaveSystem.Instance != null)
        {
            int remainingEnemies = WaveSystem.Instance.GetRemainingEnemies();
            enemyCountText.text = $"Enemies: {remainingEnemies}";
        }
    }
    
    private void UpdateButtonState()
    {
        if (startWaveButton == null || WaveSystem.Instance == null) return;
        
        bool canStart = WaveSystem.Instance.CanStartNextWave;
        startWaveButton.interactable = canStart;
        
        if (buttonText != null)
        {
            if (canStart)
            {
                int nextWaveNum = WaveSystem.Instance.CurrentWaveNumber + 1;
                buttonText.text = $"Start Wave {nextWaveNum}";
            }
            else
            {
                buttonText.text = "Please Wait...";
            }
        }
    }
    
    private void OnStartWaveButtonClicked()
    {
        if (WaveSystem.Instance != null && WaveSystem.Instance.CanStartNextWave)
        {
            WaveSystem.Instance.StartNextWave();
        }
    }
}
