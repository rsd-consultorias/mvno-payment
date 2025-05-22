# API de Assinaturas de Serviços Recorrentes

## Endpoints

### Criar Assinatura
**Rota:** `/subscriptions`  
**Método:** `POST`  
**Descrição:** Cria uma nova assinatura para o usuário.  
**Parâmetros:**
- `user_id` (string, obrigatório) - ID do usuário que está assinando.
- `plan_id` (string, obrigatório) - ID do plano de assinatura.
- `start_date` (string, obrigatório) - Data de início da assinatura no formato `YYYY-MM-DD`.
- `payment_method` (string, obrigatório) - Forma de pagamento escolhida.

---

### Alterar Dados da Assinatura
**Rota:** `/subscriptions/{subscription_id}`  
**Método:** `PUT`  
**Descrição:** Altera os dados de uma assinatura existente.  
**Parâmetros:**
- `plan_id` (string, opcional) - Novo ID do plano de assinatura.
- `payment_method` (string, opcional) - Nova forma de pagamento.
- Outros parâmetros a depender das alterações necessárias.

---

### Cancelar Assinatura
**Rota:** `/subscriptions/{subscription_id}`  
**Método:** `DELETE`  
**Descrição:** Cancela uma assinatura ativa.  
**Parâmetros:**
- `reason` (string, opcional) - Motivo do cancelamento.

---

### Desativar Assinatura Temporariamente
**Rota:** `/subscriptions/{subscription_id}/pause`  
**Método:** `POST`  
**Descrição:** Desativa a assinatura por um período determinado.  
**Parâmetros:**
- `pause_start_date` (string, obrigatório) - Data de início da pausa no formato `YYYY-MM-DD`.
- `pause_end_date` (string, obrigatório) - Data de término da pausa no formato `YYYY-MM-DD`.

---

### Reativar Assinatura
**Rota:** `/subscriptions/{subscription_id}/resume`  
**Método:** `POST`  
**Descrição:** Reativa uma assinatura que foi pausada ou cancelada.

---

### Consultar Status da Assinatura
**Rota:** `/subscriptions/{subscription_id}/status`  
**Método:** `GET`  
**Descrição:** Consulta o status atual de uma assinatura.  
**Parâmetros:**
- Nenhum (a `subscription_id` já identifica a assinatura).

---

## Exemplo de Respostas

### Para Sucesso nos Endpoints POST/PUT
```json
{
  "status": "success",
  "message": "Ação realizada com sucesso.",
  "subscription_id": "abc123"
}
```

### Para Sucesso no Endpoint GET (/subscriptions/{subscription_id}/status)
```json
{
  "subscription_id": "abc123",
  "status": "active",
  "message": "Assinatura ativa e funcionando."
}
```

### Para Erro
```json
{
  "status": "error",
  "message": "Ocorreu um erro ao processar a requisição."
}
```