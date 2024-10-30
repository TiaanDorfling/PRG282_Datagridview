﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//still need a check to ensure id stays unique
namespace PRG282_02
{
	public partial class Form1 : Form
	{
		List<Student> students = new List<Student>();

		DataTable studentTable = new DataTable();
		BindingSource src = new BindingSource();
		public Form1()
		{
			InitializeComponent();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			addStudent();
		}

		private void addStudent()
		{
			
			try
			{
				var student = new Student
				{
					Name = txtName.Text,
					Id = int.Parse(txtID.Text),
					Age = int.Parse(txtAge.Text),
					Course = txtCourse.Text
				};

				students.Add(student);
				studentTable.Rows.Add(student.Id, student.Name, student.Age, student.Course);

				save();

				// Clear input fields after adding the student
				ClearInputFields();
			}
			catch (FormatException ex)
			{
				MessageBox.Show($"Invalid input: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}

			save();
		}

		private void ClearInputFields()
		{
			txtName.Clear();
			txtID.Clear();
			txtAge.Clear();
			txtCourse.Clear();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			studentTable.Columns.Add("ID", typeof(int));
			studentTable.Columns.Add("Name", typeof(string));
			studentTable.Columns.Add("Age", typeof(int));
			studentTable.Columns.Add("Course",typeof(string));

			populateStudents();

			src.DataSource = studentTable;

			dgvStudents.DataSource = src;
		}

		private void populateStudents()
		{
			var student = new Student
			{
				Name = "Tiaan Dorfling",
				Id = 1,
				Age = 20,
				Course = "BCOMP"
			};

			students.Add(student);

			student = new Student
			{
				Name = "Tiaan Scholtz",
				Id = 2,
				Age = 20,
				Course = "BCOMP"
			};

			students.Add(student);

			student = new Student
			{
				Name = "Thian du Plessis",
				Id = 3,
				Age = 21,
				Course = "BCOMP"
			};

			students.Add(student);

			foreach (Student pupil in students)
			{
				studentTable.Rows.Add(pupil.Id, pupil.Name, pupil.Age, pupil.Course);
			}

		}

		private void display()
		{
			try
			{
				if (src.Current != null)
				{
					DataRowView currentRow = (DataRowView)src.Current;
					txtName.Text = currentRow["Name"].ToString();
					txtID.Text = currentRow["ID"].ToString();
					txtAge.Text = currentRow["Age"].ToString();
					txtCourse.Text = currentRow["Course"].ToString();
				}
				else
				{
					txtName.Text = "";
					txtID.Text = "";
					txtAge.Text = "";
					txtCourse.Text = "";
				}
			}
			catch (Exception error)
			{
				Console.WriteLine($"Error: {error.Message}");
			}
		}

		

		private void dgvStudents_SelectionChanged(object sender, EventArgs e)
		{
			display();
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			src.MoveNext();
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			src.MovePrevious();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ClearInputFields();
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			DataRowView currentRow = (DataRowView)src.Current;
			//should not be able to edit id
			//currentRow["ID"] = int.Parse(txtID.Text);
			currentRow["Name"] = txtName.Text;
			currentRow["Age"] = int.Parse(txtAge.Text);
			currentRow["Course"] = txtCourse.Text;

			currentRow.EndEdit();

			//create a new student object to replace the old one
			Student student = new Student();
			student.Id = int.Parse(currentRow["ID"].ToString());
			student.Name = currentRow["Name"].ToString();
			student.Age = int.Parse(currentRow["Age"].ToString());
			student.Course = currentRow["Course"].ToString();

			int id = int.Parse(currentRow["ID"].ToString());
	
			for (int k = 0; k < students.Count; k++)
			{
				if (students[k].Id == id)
				{
					students.RemoveAt(k);
					students.Add(student);
					break;
				}
			}


			save();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			DataRowView currentRow = (DataRowView)src.Current;
			int id = int.Parse(currentRow["ID"].ToString());
			currentRow.Delete();
			//delete student from list to save updated list
			for (int k = 0; k < students.Count; k++)
			{
				if (students[k].Id == id)
				{
					students.RemoveAt(k);
					break;
				}
			}
			save();
		}

		private void save()
		{
			Save save = new Save("student");
			save.streamWrite(students);
		}
	}
}
