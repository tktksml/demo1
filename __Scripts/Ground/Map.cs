using KyokoLib.ServiceLib;
using FTK.GamePlayLib.UnitLib;
using System.Collections.Generic;
using System.Linq;
using KyokoLib.ExtensionLib;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace FTK.GamePlayLib.MapLib
{
    public class Map : MonoBehaviour, IInitable
    {
        [Header("Cell Prefabs")]
        [SerializeField] private Cell whiteCellPrefab = null;
        [SerializeField] private Cell blackCellPrefab = null;


        [Header("Require Components")]
        [SerializeField] private Transform cellContainer = null;
        [SerializeField] private BoxCollider cellContainerCollider = null;
        [SerializeField] private NavMeshSurface unitMovableSurface = null;


        [Header("Settings")]
        [SerializeField] private Vector2Int size = Vector2Int.zero;


        [Header("Edge Require Components")]
        [SerializeField] private Transform playerSide = null;
        [SerializeField] private Transform enemySide = null;


        [Header("Warrior Settings")]
        [SerializeField] private int warriorRowCount = 2;
        [SerializeField] private int neutralZoneRowCount = 4;


        [SerializeField, HideInInspector] private List<Cell> cells = new List<Cell>();
        [SerializeField] private List<Cell> playerUnitCells = new List<Cell>();
        [SerializeField] private List<Cell> enemyUnitCells = new List<Cell>();


        private static Map self = null;








        public void CoreInit() { self = this; }


        public static Cell GetAvailableUnitCell_(Unit unit, Team team) => self.IsNotNull()?.GetAvailableUnitCell(unit, team);
        private Cell GetAvailableUnitCell(Unit unit, Team team)
        {
            //Определяем какие именно ячейки берем, игрока или врага
            var unitCells = team.IsFriend(Team.TeamFlag.Player) ? playerUnitCells : enemyUnitCells;
            unitCells = unitCells.ToList();
            //Удаляем все занятые ячейки
            unitCells.RemoveAll(cell => cell.CurrentUnit != null);


            //Сначала пытаемся найти идеальную ячейку которая совпадает с фильтром
            var bestCell = unitCells.Find(cell => cell.UnitTypeFilter.IsSame(unit.GetType()));
            if (bestCell != null)
                return bestCell;


            //Сортируем ячейки в зависимости от того кем является юнит
            if (unit.Liner == Liner.FrontLiner)
                unitCells = unitCells.OrderBy(cell => Mathf.Abs(cell.TransformCache.position.z)).ToList();
            else
                unitCells = unitCells.OrderByDescending(cell => Mathf.Abs(cell.TransformCache.position.z)).ToList();


            //Иначе возвращаем самый первую свободную ячейку
            return unitCells.Count > 0 ? unitCells[0] : null;
        }


        #if UNITY_EDITOR
        [ContextMenu("ReCreate Map")]
        private void ReCreateMap()
        {
            //Чистим старое
            cells.ForEach(cell => cell.IsNotNull()?.Destroy());
            cells.Clear();


            //Чистим старое
            playerUnitCells.Clear();
            enemyUnitCells.Clear();


            //Определяем размер ячейки (специально создал как отдельную переменную чтобы можно было в зависимости от rotation менять y и z)
            var cellSize = new Vector2(whiteCellPrefab.transform.localScale.x, whiteCellPrefab.transform.localScale.y);
            //Определяем стартовую позицию спауна ячейки, добавляем половинные размеры чтобы было по центру
            var cellStartPosition = new Vector3(-cellSize.x / 2f * size.x, 0, -cellSize.y / 2f * size.y);
            cellStartPosition += new Vector3(cellSize.x / 2f, 0, cellSize.y / 2f);
            for (int z = 0; z < size.y; z++)
            {
                //Находим сколько линий доступны для воинов
                int nonNeutralRow = (size.y - neutralZoneRowCount) / 2;
                //Определяем является ли ячейка ячейкой воина для игрока
                bool isRowPlayerWarrior = z < nonNeutralRow && z >= nonNeutralRow - warriorRowCount;
                //Определяем является ли ячейка ячейкой воина для врага
                bool isRowEnemyWarrior = z >= nonNeutralRow + neutralZoneRowCount && z < nonNeutralRow + neutralZoneRowCount + warriorRowCount;
                for (int x = 0; x < size.x; x++)
                {
                    var cellPrefab = (x + z) % 2 == 0 ? whiteCellPrefab : blackCellPrefab;
                    var cellPosition = cellStartPosition + new Vector3(cellSize.x * x, 0, cellSize.y * z);
                    var createdCell = PrefabUtility.InstantiatePrefab(cellPrefab, cellContainer) as Cell;
                    createdCell.transform.position = cellPosition;
                    cells.Add(createdCell);


                    //Добавляем союзные и вражеские ячейки для воинов
                    if (isRowPlayerWarrior)
                        playerUnitCells.Add(createdCell);
                    else if (isRowEnemyWarrior)
                        enemyUnitCells.Add(createdCell);


                    Utils.SetDirty(createdCell);
                }
            }


            //Сортируем для удобства
            playerUnitCells = playerUnitCells.OrderByDescending(cell => cell.transform.position.z).ThenBy(cell => Mathf.Abs(cell.transform.position.x)).ToList();
            enemyUnitCells = enemyUnitCells.OrderBy(cell => cell.transform.position.z).ThenBy(cell => Mathf.Abs(cell.transform.position.x)).ToList();


            enemySide.position = new Vector3(0, 0, (size.y * cellSize.x) / 2f);
            enemySide.localScale = new Vector3(cellSize.x * size.x, enemySide.localScale.y, enemySide.localScale.z);
            enemySide.position += new Vector3(0, 0, enemySide.transform.localScale.y / 2f);
            Utils.SetDirty(enemySide);


            playerSide.position = new Vector3(0, 0, -(size.y * cellSize.x) / 2f);
            playerSide.localScale = new Vector3(cellSize.x * size.x, playerSide.localScale.y, playerSide.localScale.z);
            playerSide.position -= new Vector3(0, 0, playerSide.transform.localScale.y / 2f);
            Utils.SetDirty(playerSide);


            cellContainerCollider.size = new Vector3(size.x * cellSize.x, 0.1f, size.y * cellSize.y);
            cellContainerCollider.center = new Vector3(0, -0.05f, 0);


            unitMovableSurface.BuildNavMesh();


            Utils.SetDirty(this);
            Utils.SetSceneDirty();
        }
        private void OnDrawGizmos()
        {
            var prevColor = Handles.color;


            foreach (var cell in cells.Where(cell => enemyUnitCells.Contains(cell) || playerUnitCells.Contains(cell)))
            {
                Handles.color = cell.UnitTypeFilter.IsNull() ? Color.green : cell.UnitTypeFilter.GetType().ToString().ToColor();
                Handles.DrawWireCube(cell.TransformCache.position, cell.TransformCache.rotation * cell.TransformCache.localScale);
            }


            Handles.color = prevColor;
        }
        #endif
    }
}