using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoLFA.Classes;

namespace ProyectoLFA
{
    public partial class TransitionsView : Form
    {
        private ET expressionTree;
        private FollowTable follows;
        private TransitionT transitions;

        public TransitionsView()
        {
            InitializeComponent();
        }

        public TransitionsView(ET expressionTree, FollowTable follows, TransitionT transitions)
        {
            InitializeComponent();
            this.expressionTree = expressionTree;
            this.follows = follows;
            this.transitions = transitions;

            RTXtext.Text = expressionTree.expression;
        }


        private void loadFunctions()
        {
            List<string[]> functions = expressionTree.getValuesOfNodes();

            TreeTable.Columns.Add("SIMBOLO", "SIMBOLO");
            TreeTable.Columns.Add("FIRST", "FIRST");
            TreeTable.Columns.Add("LAST", "LAST");
            TreeTable.Columns.Add("NULLABLE", "NULLABLE");

            int length = functions.Count;
            for (int i = 0; i < length; i++)
            {
                TreeTable.Rows.Add();

                TreeTable.Rows[i].Cells[0].Value = functions[i][0];
                TreeTable.Rows[i].Cells[1].Value = functions[i][1];
                TreeTable.Rows[i].Cells[2].Value = functions[i][2];
                TreeTable.Rows[i].Cells[3].Value = functions[i][3];
            }
        }

        private void loadFollows()
        {
            FollowTable.Columns.Add("SIMBOLO", "SIMBOLO");
            FollowTable.Columns.Add("FOLLOW", "FOLLOW");

            for (int rowIndex = 1; rowIndex < follows.nodes.Count; rowIndex++)
            {
                var item = follows.nodes[rowIndex];
                FollowTable.Rows.Add();
                FollowTable.Rows[rowIndex - 1].Cells[0].Value = item.character;
                FollowTable.Rows[rowIndex - 1].Cells[1].Value = string.Join(",", item.follows);
            }

            DataGridViewColumn column1 = FollowTable.Columns[0];
            DataGridViewColumn column2 = FollowTable.Columns[1];
        }

        private void loadTransitions()
        {
            int indexCount = 1;
            //Key=symbol
            ////value=index
            Dictionary<string, int> index = new Dictionary<string, int>();

            //Columns
            TransitionsTable.Columns.Add("ESTADO", "ESTADO");
            foreach (var item in transitions.symbolsList)
            {
                index.Add(item, indexCount);
                TransitionsTable.Columns.Add(item, item);
                indexCount++;
            }

            //Rows (States)
            for (int i = 0; i < transitions.states.Count; i++)
            {
                TransitionsTable.Rows.Add();
                var item = string.Join(",", transitions.states[i]);

                TransitionsTable.Rows[i].Cells[0].Value = item;
            }
            //Cells (Transitions)
            for (int i = 0; i < transitions.states.Count; i++)
            {
                foreach (var item in transitions.transitions[i])
                {
                    TransitionsTable.Rows[i].Cells[item.symbol].Value = string.Join(",", item.nodes);
                    if (string.IsNullOrEmpty(TransitionsTable.Rows[i].Cells[item.symbol].Value.ToString())) //Fill the cells that are null with ---
                    {
                        TransitionsTable.Rows[i].Cells[item.symbol].Value = "---";
                    }
                }
            }

        }
        private void TransitionsView_Load(object sender, EventArgs e)
        {
            loadFunctions();
            loadFollows();
            loadTransitions();
        }
    }
}
