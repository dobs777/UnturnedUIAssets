# MenuUI - Plugin RocketMod para Unturned

Plugin completo de menu UI style Vanilla com abas: KITS, VIPS, HOMES, INFORMACOES.

## Comandos

| Comando       | Alias          | Abre               |
|--------------|----------------|--------------------|
| `/menu`      | `/m`           | Menu (1a aba)      |
| `/menu kits` | -              | Aba Kits           |
| `/kits`      | -              | Aba Kits           |
| `/vips`      | `/loja`        | Aba VIPs           |
| `/homes`     | -              | Aba Homes          |
| `/info`      | `/informacoes` | Aba Informacoes    |

## Como compilar

1. Visual Studio 2022, projeto C# (.NET Framework 4.8)
2. Referencias: `Rocket.Core.dll`, `Rocket.Unturned.dll`, `Assembly-CSharp.dll`, `UnityEngine.dll`
3. Adicione todos os `.cs` da pasta `Plugin/`
4. Compile -> `MenuUI.dll` -> coloque em `Rocket/Plugins/`

## Configuracao

Edite `MenuUI.configuration.xml` apos a primeira execucao.
Mude `EffectID` para o ID do seu item no Workshop.

## Slots de UI

| Aba          | Slots | Layout         |
|-------------|-------|----------------|
| Kits         | 12    | Grid 4 colunas |
| VIPs         | 6     | Grid 3 colunas |
| Homes        | 12    | Lista vertical |
| Informacoes  | 6     | Grid 3 colunas |

## Workshop

1. Abra o Unity project em `dobs777/Unturned-UI---Unityproject`
2. Prefab esta em `BlazingFlame/MenuUI/MenuUI.prefab`
3. Bundle com Bundle Tool do Unturned
4. Coloque em `[WorkshopItem]/Bundles/Effects/MenuUI/`
5. Publique no Workshop, copie o Effect ID
6. Coloque o ID em `EffectID` na configuracao

## Integracao com plugin de Homes

Para usar homes dinamicas do jogador, sobrescreva `GetPlayerHomes` em `MenuUIManager`:

```csharp
protected override List<MenuItemConfig> GetPlayerHomes(UnturnedPlayer player)
{
    // Exemplo - adapte ao seu plugin de homes:
    return MeuHomesPlugin.GetHomes(player.Id)
        .Select(h => new MenuItemConfig { Name = h.Name, Command = "/home " + h.Name })
        .ToList();
}
```
