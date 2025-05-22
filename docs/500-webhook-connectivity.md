## Webhook para Eventos de uma Plataforma de Conectividade

### Endpoint: Receber Eventos da Plataforma
**Rota:** `/webhook/connectivity-platform`  
**Método:** `POST`  
**Descrição:** Endpoint que recebe notificações de eventos gerados por uma plataforma de conectividade (ex.: IoT, integrações).  
**Headers:**
- `Authorization` (string, obrigatório) - Token para validar as requisições da plataforma.

**Parâmetros (no corpo da requisição):**
- `event_type` (string, obrigatório) - Tipo do evento. Exemplos: `device_connected`, `device_disconnected`, `error_reported`.
- `device_id` (string, obrigatório) - Identificador único do dispositivo.
- `timestamp` (string, obrigatório) - Data e hora do evento no formato ISO 8601.
- `metadata` (object, opcional) - Informações adicionais específicas do evento.

### Exemplo de Evento Recebido
```json
{
  "event_type": "device_connected",
  "device_id": "device_7890",
  "timestamp": "2025-03-23T21:30:00Z",
  "metadata": {
    "ip_address": "192.168.1.10",
    "connection_strength": "strong"
  }
}
```

### Exemplo de Resposta
```json
{
  "status": "success",
  "message": "Evento processado com sucesso."
}
```