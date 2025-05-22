# API de Invoicing

## Endpoints

### Criar Fatura
**Rota:** `/invoices`  
**Método:** `POST`  
**Descrição:** Cria uma nova fatura para cobrança.  
**Parâmetros:**
- `user_id` (string, obrigatório) - ID do usuário associado à fatura.
- `amount` (float, obrigatório) - Valor da fatura.
- `due_date` (string, obrigatório) - Data de vencimento no formato `YYYY-MM-DD`.
- `description` (string, opcional) - Descrição ou detalhes da fatura.

---

### Consultar Fatura
**Rota:** `/invoices/{invoice_id}`  
**Método:** `GET`  
**Descrição:** Consulta os detalhes de uma fatura específica.  
**Parâmetros:**  
- Nenhum (a `invoice_id` já identifica a fatura).

---

### Atualizar Fatura
**Rota:** `/invoices/{invoice_id}`  
**Método:** `PUT`  
**Descrição:** Atualiza os dados de uma fatura existente.  
**Parâmetros:**
- `amount` (float, opcional) - Novo valor da fatura.
- `due_date` (string, opcional) - Nova data de vencimento.
- `description` (string, opcional) - Nova descrição da fatura.

---

### Cancelar Fatura
**Rota:** `/invoices/{invoice_id}`  
**Método:** `DELETE`  
**Descrição:** Cancela uma fatura antes de seu pagamento.

---

### Gerar Pagamento de Fatura
**Rota:** `/invoices/{invoice_id}/pay`  
**Método:** `POST`  
**Descrição:** Gera um pagamento para uma fatura específica.  
**Parâmetros:**
- `payment_method` (string, obrigatório) - Método de pagamento. Valores permitidos: `pix`, `credit_card`, `boleto`.
- `payment_details` (object, opcional) - Informações adicionais dependendo do método selecionado:
  - Para `credit_card`: número do cartão, validade, CVV.
  - Para `pix`: chave Pix.
  - Para `boleto`: nenhum detalhe adicional necessário.

---

### Consultar Status da Fatura
**Rota:** `/invoices/{invoice_id}/status`  
**Método:** `GET`  
**Descrição:** Consulta o status atual de uma fatura.  
**Parâmetros:**  
- Nenhum (a `invoice_id` já identifica a fatura).

---

### Gerenciamento de Pagamentos Recorrentes
**Rota:** `/invoices/recurring`  
**Método:** `POST`  
**Descrição:** Configura a recorrência de pagamentos para o usuário.  
**Parâmetros:**
- `user_id` (string, obrigatório) - ID do usuário.
- `amount` (float, obrigatório) - Valor recorrente.
- `interval` (string, obrigatório) - Intervalo de recorrência (ex.: `monthly`, `yearly`).
- `start_date` (string, obrigatório) - Data de início no formato `YYYY-MM-DD`.

---

### Alterar Configurações de Pagamento Recorrente
**Rota:** `/invoices/recurring/{recurring_id}`  
**Método:** `PUT`  
**Descrição:** Atualiza configurações de pagamentos recorrentes.  
**Parâmetros:**
- `amount` (float, opcional) - Novo valor recorrente.
- `interval` (string, opcional) - Novo intervalo de recorrência.
- `start_date` (string, opcional) - Nova data de início.

---

### Cancelar Pagamento Recorrente
**Rota:** `/invoices/recurring/{recurring_id}`  
**Método:** `DELETE`  
**Descrição:** Cancela pagamentos recorrentes configurados.

---

## Exemplo de Respostas

### Para Sucesso nos Endpoints POST/PUT
```json
{
  "status": "success",
  "message": "Ação realizada com sucesso.",
  "invoice_id": "invoice123"
}
```

### Para Sucesso no Endpoint GET (/invoices/{invoice_id}/status)
```json
{
  "invoice_id": "invoice123",
  "status": "paid",
  "message": "Fatura paga com sucesso."
}
```

### Para Erro
```json
{
  "status": "error",
  "message": "Ocorreu um erro ao processar a requisição."
}
```