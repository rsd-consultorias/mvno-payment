# Webhooks API

## Webhook para Eventos do Payment Gateway

### Endpoint: Receber Eventos de Pagamento
**Rota:** `/webhook/payment-gateway`  
**Método:** `POST`  
**Descrição:** Endpoint que recebe notificações do *payment gateway* sobre eventos relacionados a pagamentos.  
**Headers:**
- `Authorization` (string, obrigatório) - Token para validar as requisições do gateway.

**Parâmetros (no corpo da requisição):**
- `event_type` (string, obrigatório) - Tipo do evento. Exemplos: `payment_succeeded`, `payment_failed`, `payment_refunded`.
- `transaction_id` (string, obrigatório) - ID único da transação.
- `amount` (float, obrigatório) - Valor relacionado à transação.
- `currency` (string, obrigatório) - Moeda da transação.
- `timestamp` (string, obrigatório) - Data e hora do evento no formato ISO 8601.
- `details` (object, opcional) - Informações adicionais, dependendo do tipo de evento.

### Exemplo de Evento Recebido
```json
{
  "event_type": "payment_succeeded",
  "transaction_id": "txn_123456",
  "amount": 199.99,
  "currency": "BRL",
  "timestamp": "2025-03-23T20:00:00Z",
  "details": {
    "payment_method": "credit_card",
    "card_last4": "1234"
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