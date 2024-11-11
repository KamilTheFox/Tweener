# 🦊 Unity Fox Tweener

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Unity](https://img.shields.io/badge/Unity-2021.3%2B-brightgreen.svg)

## 📝 Описание
Unity Fox Tweener - легковесная система анимации и интерполяции для Unity, позволяющая создавать плавные переходы и анимации прямо из кода. Разработана с фокусом на производительность и простоту использования.

## ✨ Основные возможности
- 🎯 Плавное перемещение объектов (Move)
- 🔄 Вращение (Rotate)
- 📏 Масштабирование (Scale)
- 🎨 Анимация цвета (Color)
- ⚡ Поддержка различных кривых анимации
- 🔗 Последовательные и параллельные анимации
- ⏱️ Настраиваемые функции интерполяции

## 🚀 Быстрый старт
```csharp
// Перемещение объекта с возвращением в начальное положение используя квадратичную функцию прогресса
private IExpansionTween move;
move = Tween.AddPosition(obj2.transform, torward, time)
.ChangeLoop(TypeLoop.PingPong)
.ChangeEase(Ease.CubicDegree);

//Инвертировать прогрессию
move.ReverseProgress();
//Остановить Твин
Tween.Stop(move);

//Воспроизведение события, когда твин будет остановлен или когда объект будет уничтожен
move.ToCompletion(Stoped, CallWhenDestroy: true);
