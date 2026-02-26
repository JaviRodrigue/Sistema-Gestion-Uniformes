# Configuración de Visual Studio para Manejo de Excepciones

## Problema
Visual Studio puede pausar la ejecución cuando se lanza una excepción, incluso si está siendo manejada correctamente con try-catch. Esto puede dar la impresión de que la aplicación se está "rompiendo" cuando en realidad la excepción está siendo capturada.

## Solución: Configurar Exception Settings

### Opción 1: Deshabilitar "Break When Thrown" para ExcepcionDominio

1. **Abrir Exception Settings:**
   - Ve a `Debug > Windows > Exception Settings` (Ctrl+Alt+E)
   - O presiona `Ctrl+Alt+E`

2. **Configurar Common Language Runtime Exceptions:**
   - Busca `Common Language Runtime Exceptions`
   - Expande el nodo
   - Busca o agrega `VentasApp.Domain.Base.ExcepcionDominio`
   - **Desmarca** la casilla para que NO pause cuando se lance esta excepción

3. **Verificar configuración:**
   - La casilla junto a `ExcepcionDominio` debe estar **desmarcada**
   - Esto permite que la excepción sea lanzada y capturada sin pausar el debugger

### Opción 2: Deshabilitar "Just My Code"

Si prefieres que el debugger no se detenga en ninguna excepción manejada:

1. Ve a `Tools > Options`
2. Navega a `Debugging > General`
3. **Marca** la opción `Enable Just My Code`
4. Esto hace que el debugger solo pause en excepciones no manejadas en tu código

### Opción 3: Continuar la Ejecución

Si el debugger se detiene en la excepción:
- Simplemente presiona `F5` (Continue) o haz clic en "Continue"
- La excepción será capturada por el try-catch y se mostrará el MessageBox
- La aplicación NO se cerrará

## Verificación del Manejo de Excepciones

### La aplicación está correctamente protegida con:

1. **Try-Catch en GuardarVentaAsync():**
   ```csharp
   catch (ExcepcionDominio ex) ? MessageBox Warning
   catch (Exception ex) ? MessageBox Error
   ```

2. **Try-Catch en Guardar_Click():**
   ```csharp
   try { await GuardarVentaAsync(); }
   catch (ExcepcionDominio) ? MessageBox Warning
   catch (Exception) ? MessageBox Error
   ```

3. **Manejador Global en App.xaml.cs:**
   ```csharp
   App_DispatcherUnhandledException
   e.Handled = true ? Previene cierre de aplicación
   ```

## Prueba sin Debugger

Para verificar que la aplicación funciona correctamente:

1. **Compilar en Release:**
   - Cambia a configuración `Release`
   - Compila el proyecto

2. **Ejecutar sin Debugger:**
   - Presiona `Ctrl+F5` (Start Without Debugging)
   - O ejecuta el .exe directamente desde `bin\Release\net8.0-windows\`

3. **Resultado esperado:**
   - Al intentar agregar producto sin stock suficiente
   - Se muestra un MessageBox con título "Validación"
   - El usuario hace clic en OK
   - La ventana permanece abierta
   - El usuario puede corregir y volver a intentar

## Conclusión

Si ves el stack trace de la excepción en la ventana de Output de Visual Studio, esto es **normal** cuando el debugger está adjunto. La excepción está siendo manejada correctamente y la aplicación **NO se cierra**.

Para una experiencia sin interrupciones durante el desarrollo, sigue la **Opción 1** para deshabilitar el break en `ExcepcionDominio`.
