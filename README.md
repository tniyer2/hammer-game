
# hammer-game

This is a game made using Unity where you
can fight enemies with a hammer and a crossbow.

Build
===
You can play the game on a browser using the [prebuilt WebGL build](https://github.com/tniyer2/hammer-game/releases/tag/1.0.0).

Once unzipped, serve the static content locally.
You can do this by using python:
```
python3 -m http.server
```
Then navigate to "localhost:PORT" in a browser.
PORT being whatever port http.server opened on (most likely 8000).

Instructions
===
Press escape to enter the menu and equip your weapons
in the inventory by clicking on them.
Press shift to switch between your primary and secondary weapons.
You can move around using the arrow keys or "wasd".
You can jump and enter doors using the spacebar.

The hammer charges up by holding the right mouse button
or "x". When the charge bar (red) is completely filled you can release to deal damage.
You can use the same controls to shoot arrows from the crossbow.
The farther away the enemy is from the crossbow, the more damage is dealt.
When the weapon UI bar (orange) is completely charged, you can do a special attack.
This is activated by pressing the left mouse button or "z".
