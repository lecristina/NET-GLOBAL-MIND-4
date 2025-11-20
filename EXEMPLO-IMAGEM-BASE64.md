# 📸 Como Criar uma Imagem Base64 Válida

## Formato Aceito

O endpoint `/api/v1.0/ML/imagem/classificar` aceita imagens em **Base64** nos seguintes formatos:

- ✅ **JPEG/JPG** (recomendado)
- ✅ **PNG**
- ✅ **GIF**

**Tamanho máximo:** 10MB

## Formatos de Base64 Aceitos

O endpoint aceita dois formatos:

### Formato 1: Com prefixo (Data URL)
```json
{
  "imagemBase64": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD...",
  "descricao": "Minha mesa de trabalho"
}
```

### Formato 2: Apenas Base64 (sem prefixo)
```json
{
  "imagemBase64": "/9j/4AAQSkZJRgABAQEAYABgAAD...",
  "descricao": "Minha mesa de trabalho"
}
```

## 🛠️ Como Converter uma Imagem para Base64

### Opção 1: Usando PowerShell (Windows)

```powershell
# Converter imagem para base64
$imagePath = "C:\caminho\para\sua\imagem.jpg"
$imageBytes = [System.IO.File]::ReadAllBytes($imagePath)
$base64String = [System.Convert]::ToBase64String($imageBytes)

# Com prefixo data URL
$dataUrl = "data:image/jpeg;base64,$base64String"
Write-Host $dataUrl
```

### Opção 2: Usando Python

```python
import base64

# Ler imagem e converter para base64
with open("imagem.jpg", "rb") as image_file:
    encoded_string = base64.b64encode(image_file.read()).decode('utf-8')
    
# Com prefixo data URL
data_url = f"data:image/jpeg;base64,{encoded_string}"
print(data_url)
```

### Opção 3: Usando JavaScript/Node.js

```javascript
const fs = require('fs');

// Ler imagem e converter para base64
const imageBuffer = fs.readFileSync('imagem.jpg');
const base64String = imageBuffer.toString('base64');

// Com prefixo data URL
const dataUrl = `data:image/jpeg;base64,${base64String}`;
console.log(dataUrl);
```

### Opção 4: Online (Ferramentas Web)

1. Acesse: https://www.base64-image.de/
2. Faça upload da sua imagem
3. Copie o resultado

## 📝 Exemplo Completo de Requisição

### Exemplo 1: Imagem JPEG (com prefixo)

```json
{
  "imagemBase64": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwCdABmX/9k=",
  "descricao": "Minha mesa de trabalho, um pouco desorganizada"
}
```

### Exemplo 2: Imagem PNG (sem prefixo)

```json
{
  "imagemBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==",
  "descricao": "Ambiente de trabalho organizado"
}
```

## 🧪 Testando com cURL

```bash
curl -X POST "https://seu-servidor/api/v1.0/ML/imagem/classificar" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_JWT" \
  -d '{
    "imagemBase64": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD...",
    "descricao": "Minha mesa de trabalho"
  }'
```

## 🧪 Testando com PowerShell

```powershell
# Converter imagem para base64
$imagePath = "C:\caminho\para\imagem.jpg"
$imageBytes = [System.IO.File]::ReadAllBytes($imagePath)
$base64String = [System.Convert]::ToBase64String($imageBytes)
$dataUrl = "data:image/jpeg;base64,$base64String"

# Criar JSON
$body = @{
    imagemBase64 = $dataUrl
    descricao = "Minha mesa de trabalho"
} | ConvertTo-Json

# Fazer requisição
$headers = @{
    "Authorization" = "Bearer SEU_TOKEN_JWT"
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "https://seu-servidor/api/v1.0/ML/imagem/classificar" `
    -Method POST `
    -Headers $headers `
    -Body $body
```

## ⚠️ Validações do Endpoint

O endpoint valida automaticamente:

1. ✅ **Formato da imagem**: Deve ser JPEG, PNG ou GIF (verificado pelos magic bytes)
2. ✅ **Tamanho**: Máximo de 10MB
3. ✅ **Base64 válido**: Deve ser uma string base64 válida

## 🔍 Imagem Base64 Mínima para Teste

Aqui está uma imagem 1x1 pixel em JPEG (muito pequena, apenas para teste):

```json
{
  "imagemBase64": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwCdABmX/9k=",
  "descricao": "Imagem de teste"
}
```

## 💡 Dicas

1. **Use imagens pequenas para testes**: Imagens menores processam mais rápido
2. **Comprima antes de converter**: Reduza o tamanho da imagem antes de converter para base64
3. **Use JPEG para fotos**: JPEG é melhor para fotos reais
4. **Use PNG para gráficos**: PNG é melhor para imagens com texto ou gráficos simples

## 🚨 Erros Comuns

### Erro: "Imagem inválida ou formato não suportado"
- ❌ Base64 malformado
- ❌ Formato de imagem não suportado (ex: BMP, TIFF)
- ❌ Imagem corrompida
- ✅ **Solução**: Use JPEG, PNG ou GIF válidos

### Erro: "Imagem muito grande"
- ❌ Arquivo maior que 10MB
- ✅ **Solução**: Comprima a imagem antes de converter

### Erro: "Base64 string vazia"
- ❌ String base64 vazia ou nula
- ✅ **Solução**: Certifique-se de que a imagem foi convertida corretamente

