# DOMAIN EVENTS

## Use Case

```text
Criar um cadastro de Pedidos
- Toda vez que for adiciona um item de pedido, deve gerar um evento e descontar do estoque do produto
- Toda vez que for remover um item de pedido, deve gerar um evento e aumentar o estoque do produto
- Toda vez que o pedido sofrer uma mudança de Status, deve gerar um evento que irá enviar um e-mail ao usuário
```

## References

- [Eventos de Domínio | DDD do jeito certo | Parte 07](https://www.youtube.com/watch?v=_By3QRBMHSo)
- [How to Use the Domain Event Pattern](https://www.youtube.com/watch?v=BimfDeDV4yU)
- [Using Domain Events To Build A Decoupled System The Scales](https://www.youtube.com/watch?v=AHzWJ_SMqLo)
- [Don't Make This Common Domain Events Mistake](https://www.youtube.com/watch?v=5L2BUuSdZBI)
- [Transactional Outbox Pattern](https://www.youtube.com/watch?v=XALvnX7MPeo)
