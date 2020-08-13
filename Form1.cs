using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public enum Operation{
        none,
        add,
        substruct,
        multiply,
        divide
    }

    public partial class Form1 : Form
    {
        Operation operation = Operation.none;
        Operation lastOperation = Operation.none;

        bool isOperatorPressed = false;
        bool isEqualPressd = false;

        double result = 0;
        double value = 0;

        string operationSign = "";
        public Form1()
        {
            InitializeComponent();
            this.Focus();
        }

        private void buttonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string str = button.Text;
            makeTextBoxResult(str);
        }


        private void operatorClick(object sender, EventArgs e)
        {
            Button button = (Button) sender;

            setOperator(button.Text);

        }

        private void setOperator(string op)
        {
            if (!isEqualPressd)
                value = Double.Parse(textBoxResult.Text);


            switch (op)
            {
                case "+":
                    operation = Operation.add;
                    break;
                case "-":
                    operation = Operation.substruct;
                    break;
                case "*":
                    operation = Operation.multiply;
                    break;
                case "/":
                    operation = Operation.divide;
                    break;
                default: break;
            }

            operationSign = op;
            if (isOperatorPressed)
            {
                changeText();
                return;
            }

            if (isEqualPressd)
            {
                labelOperation.Text = result + op;
                isEqualPressd = false;
                return;
            }

            doOperation(value);
            makeText();

            isOperatorPressed = true;
        }

        private void changeText()
        {
            switch (operation)
            {
                case Operation.add:
                case Operation.substruct:
                case Operation.multiply:
                case Operation.divide:
                    labelOperation.Text = labelOperation.Text.Remove(labelOperation.Text.Length - 1) + operationSign;
                    break;
                default: break;
            }
        }

        private void makeText()
        {
            switch (operation)
            {
                case Operation.add:
                case Operation.substruct:
                case Operation.multiply:
                case Operation.divide:
                    labelOperation.Text = labelOperation.Text +
                                textBoxResult.Text +
                                operationSign;
                    break;
                default: break;
            }
            textBoxResult.Text = result.ToString();
        }

        private void makeTextBoxResult(string str)
        {
            if (operation != Operation.none)
            {
                textBoxResult.Clear();
                lastOperation = operation;
                operation = Operation.none;
            }
            if (isEqualPressd && !isOperatorPressed)
            {
                reset();
                isEqualPressd = false;
                lastOperation = operation;
                operation = Operation.none;
            }
            if (textBoxResult.Text.Length > 10) return;

 

            if (str == "," &&
                textBoxResult.Text.Contains(",")
                )
                return;

            if (textBoxResult.Text == "0")
                if (str == ",") textBoxResult.Text = textBoxResult.Text + ",";
                else textBoxResult.Text = str;
            else textBoxResult.Text = textBoxResult.Text + str;

            isOperatorPressed = false;
        }
        private void doOperation(double val)
        {
            switch (lastOperation)
            {
                case Operation.none:
                    result = val;
                    break;
                case Operation.add:
                    result += val;
                    break;
                case Operation.substruct:
                    result -= val;
                    break;
                case Operation.multiply:
                    result *= val;
                    break;
                case Operation.divide:
                    result /= val;
                    break;
                default: break;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            textBoxResult.Text = "0";
        }

        private void reset()
        {
            textBoxResult.Text = "0";
            labelOperation.Text = "";
            result = 0;
            value = 0;
            operation = Operation.none;
            lastOperation = Operation.none;
            operationSign = "";
        }

        private void allCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void equal_click(object sender, EventArgs e)
        {
            calculate();
        }

        private void calculate()
        {
            double lastresult = result;
            if (!isEqualPressd && isOperatorPressed)
            {
                value = Double.Parse(textBoxResult.Text);
                lastOperation = operation;
                doOperation(value);
            }

            if (!isEqualPressd && !isOperatorPressed)
            {
                value = Double.Parse(textBoxResult.Text);
                doOperation(value);
            }

            if (isEqualPressd) doOperation(value);

            textBoxResult.Text = result.ToString();
            if (!isEqualPressd) labelOperation.Text = labelOperation.Text + value + "=";
            else labelOperation.Text = lastresult + "+" + value + "=";
            isEqualPressd = true;
            isOperatorPressed = false;
        }

        private void keyDown(object sender, PreviewKeyDownEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    reset();
                    break;
                case Keys.Oemplus:
                case Keys.Enter:
                    calculate();
                    break;
                case Keys.Add:
                    setOperator("+");
                    break;
                case Keys.Subtract:
                    setOperator("-");
                   break;
                case Keys.Multiply:
                    setOperator("*");
                    break;
                case Keys.Divide:
                    setOperator("/");
                    break;

                default:
                    int key = (int)e.KeyData;
                    int val = -1;
                    if (key > 47 && key < 58)
                        val = key - 48;
                    if (key > 95 && key < 106)
                        val = key - 96;
                    if (val == -1)
                        return;
                    makeTextBoxResult(val.ToString());
                    break;
            }
        }

        private void focusButton(object sender, EventArgs e)
        {
            return;
        }
    }
}
