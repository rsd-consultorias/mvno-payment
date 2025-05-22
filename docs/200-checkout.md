# API de Checkout

## Endpoints

### Captura de Dados Pessoais
**Rota:** `/checkout/personal-data`  
**Método:** `POST`  
**Descrição:** Captura informações pessoais do cliente.  
**Parâmetros:**
- `name` (string, obrigatório) - Nome do cliente.
- `email` (string, obrigatório) - Email do cliente.
- `phone` (string, opcional) - Telefone do cliente.

---

### Captura de Endereço de Faturamento
**Rota:** `/checkout/billing-address`  
**Método:** `POST`  
**Descrição:** Captura o endereço de faturamento do cliente.  
**Parâmetros:**
- `street` (string, obrigatório) - Rua.
- `number` (string, obrigatório) - Número.
- `neighborhood` (string, obrigatório) - Bairro.
- `city` (string, obrigatório) - Cidade.
- `state` (string, obrigatório) - Estado.
- `postal_code` (string, obrigatório) - CEP.

---

### Captura de Forma de Pagamento
**Rota:** `/checkout/payment-method`  
**Método:** `POST`  
**Descrição:** Captura a forma de pagamento escolhida pelo cliente.  
**Parâmetros:**
- `method` (string, obrigatório) - Método de pagamento. Valores permitidos: `pix`, `credit_card`, `boleto`.
- `details` (object, opcional) - Informações adicionais dependendo do método selecionado:
  - Para `credit_card`: número do cartão, validade, CVV.
  - Para `pix`: chave Pix.
  - Para `boleto`: nenhum detalhe adicional necessário.

---

### Consulta do Status do Processo de Checkout
**Rota:** `/checkout/status`  
**Método:** `GET`  
**Descrição:** Consulta o status atual do processo de checkout.  
**Parâmetros:**
- `transaction_id` (string, obrigatório) - ID único da transação para identificar o processo.

---

## Exemplo de Resposta

### Para Sucesso nos Endpoints POST
```json
{
  "status": "success",
  "message": "Dados salvos com sucesso."
}
```

### Para Sucesso no Endpoint GET (/checkout/status)
```json
{
  "transaction_id": "123456",
  "status": "completed",
  "message": "Checkout finalizado com sucesso."
}
```

### Para Erro
```json
{
  "status": "error",
  "message": "Informações insuficientes ou inválidas."
}
```
----

## Comparativo de gateways

| Gateway de Pagamento      | Recursos Principais                                                                            | Suporte Global | Foco Regional                        | Ideal Para                                                                                       |
|---------------------------|-----------------------------------------------------------------------------------------------|----------------|-------------------------------------|--------------------------------------------------------------------------------------------------|
| **Stripe**                | Cobrança recorrente, APIs avançadas, suporte para diversas moedas                             | Sim            | Global                              | Empresas com alto volume de transações internacionais e desenvolvedores focados em personalização |
| **Adyen**                 | Ampla gama de métodos de pagamento, suporte para várias moedas                                | Sim            | Global                              | Grandes empresas com operações internacionais                                                   |
| **Authorize.Net**         | Segurança robusta, escalabilidade, suporte para cartões e pagamento recorrente                | Parcial        | Principalmente nos EUA              | Negócios de médio e grande porte focados no mercado norte-americano                              |
| **Porto Bank (Orquestrador de Pagamentos)** | Integração de Pix, cartões, boletos, tokenização e gestão de pagamento recorrente | Não            | Brasil                              | Empresas brasileiras que buscam soluções completas com suporte local e integração simplificada   |
| **PayPal (Braintree) ???**    | Suporte para pagamentos online, integração com PayPal, modelos recorrentes                    | Sim            | Global                              | Empresas que desejam um gateway com forte suporte global                                         |
| **Mercado Pago ???**          | Integração local, suporte para Pix, boleto bancário e cartões                                 | Não            | América Latina                      | Empresas que operam na América Latina                                                           |
| **PagSeguro ???**             | Métodos de pagamento adaptados ao mercado local, suporte para assinaturas                     | Não            | Brasil                              | Empresas brasileiras que desejam soluções personalizadas para o mercado local                    |

