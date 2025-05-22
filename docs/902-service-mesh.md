# Service Mesh

## Policies no Istio

### VirtualService com Retry

```yaml
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: catalog-api
spec:
  hosts:
    - catalog-api
  http:
    - retries:
        attempts: 3
        perTryTimeout: 2s
      route:
        - destination:
            host: catalog-api
```

### DestinationRule com Circuit Breaker

```yaml
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: catalog-api
spec:
  host: catalog-api
  trafficPolicy:
    connectionPool:
      http:
        http1MaxPendingRequests: 1
        maxRequestsPerConnection: 2
    outlierDetection:
      consecutiveErrors: 5
      interval: 10s
      baseEjectionTime: 30s
```

Você pode salvar essas configurações em arquivos .yaml separados e aplicá-los no cluster usando o comando 

```bash
kubectl apply -f <arquivo>.
```
