# 🚀 GitHub Actions - EasyPOS

## 📋 Workflows Configurados

### **1. CI/CD Pipeline** (`ci-cd.yml`)
- **Trigger**: Push a main/develop y Pull Requests
- **Acciones**:
  - ✅ Build de la solución
  - ✅ Ejecución de pruebas
  - ✅ Análisis de calidad de código
  - ✅ Escaneo de seguridad con Trivy
  - ✅ Upload de artefactos

### **2. Documentation Check** (`documentation.yml`)
- **Trigger**: Cambios en carpeta Documentation/
- **Acciones**:
  - ✅ Verificar existencia de archivos de documentación
  - ✅ Validar sintaxis Markdown
  - ✅ Asegurar contenido en archivos

### **3. Release** (`release.yml`)
- **Trigger**: Tags de versión (v1.0.0, v1.1.0, etc.)
- **Acciones**:
  - ✅ Build en modo Release
  - ✅ Publicar Web API
  - ✅ Crear release en GitHub
  - ✅ Subir artefactos como zip

---

## 🔧 Variables de Entorno

- **DOTNET_VERSION**: 10.0.x
- **SOLUTION_FILE**: EasyPOS.slnx
- **GITHUB_TOKEN**: Token automático de GitHub

---

## 📊 Monitoreo

Los workflows se pueden monitorear en:
- **GitHub Actions Tab**: https://github.com/gemelosenpai/EasyPOS_WebAPI/actions
- **Status Badges**: Se pueden agregar al README principal

---

## 🚀 Uso

### **Para hacer release:**
```bash
git tag v1.0.0
git push origin v1.0.0
```

### **Para verificar CI:**
```bash
git push origin main
# o crear Pull Request
```

### **Para verificar documentación:**
```bash
# Modificar archivos en Documentation/
git add Documentation/
git commit -m "docs: Update documentation"
git push origin main
```

---

## 📝 Notas

- Los workflows se ejecutan automáticamente
- Los artefactos se guardan por 30 días
- Los releases son permanentes
- La documentación debe mantenerse actualizada
