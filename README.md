# FallTimeGame

!!!WARNING!!! Please download zip archive from here, because Zenject issue after clone can reproduced

Zip проекта: https://drive.google.com/open?id=17qB36lsoZcsHRTo_7pdPWYFn7V5uk5k0

Затраченное на проект время: 21 час.
Подробнее смотрите в Google Doc ссылка: https://docs.google.com/spreadsheets/d/1LO0fbMPVsmha0xtcuYZ1ADOH1g7YSmUXt1FpCZtQ_7k/edit#gid=0

Описание проекта:

Версия Unity: 2018.2.6f1

Используемые плагины: 

 1. Zenject
 2. UniRx
 3. DoTween
 4. GitHub for Unity
 5. Cinemachine
 6. TextMeshPro
 7. UI Controller
 8. Free Platform Game Assets
 
 
Описание геймплея:

При запуске игры появляется стартовое меню, после нажатия кнопки "Start Game" появляется персонаж и сундук.

Управление персонажем осуществляется двумя кнопками (вверх / вниз) и поворотом экрана мобильного телефона. Персонаж выполнен в качестве ragdoll объекта.
У персонажа есть определенное количество здоровья, разные ловушки отнимают различное его количество. 

Существует 3 вида ловушек / врагов:

Шипы, которые торчат со стен
Лезвия, которые перемещаются между двумя точками
Пушки, которые выпускают снаряды очередями
Игроку необходимо спуститься вниз, подобрать сундук и подняться обратно наверх, после этого появиться всплывающее окно, что пользователь победил. Если пользователь погиб, то он может перезапустить игру, также есть меню паузы.

Enviroment построен с помощью TileMap системы. 

Для повышения производительности использовалась Pooling система (используется веревкой для создания сегментов)

![Alt Text](https://github.com/Goncharuk-Nikita/FallTimeGame/blob/master/2018-12-31_17-24-41.gif)
