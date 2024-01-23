using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace B2B.Forms.Register
{
    public partial class RegisterForm : Form
    {
        private readonly RegisterService registerService;
        private readonly IdentityDataContext _context;

        public RegisterForm(RegisterService registerService, IdentityDataContext context)
        {
            // Inicializa os componentes do formul�rio
            InitializeComponent();

            this.registerService = registerService;
            this._context = context;
        }

        private async void btn_salvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Valida��o do e-mail
                if (!IsValidEmail(input_email.Text))
                {
                    MessageBox.Show("Por favor, insira um e-mail v�lido.");
                    return;
                }

                // Verifica se o e-mail j� existe
                if (AlreadyExists(input_email.Text))
                {
                    MessageBox.Show("Usu�rio com este e-mail j� existe.");
                    return;
                }

                // Valida��o das senhas
                string senha = input_senha.Text;
                string confirmacaoSenha = input_confirmarSenha.Text;

                // Verifica se as senhas coincidem
                if (senha != confirmacaoSenha)
                {
                    MessageBox.Show("As senhas n�o coincidem.");
                    return;
                }

                // Verifica se a senha � forte
                if (!IsStrongPassword(senha))
                {
                    MessageBox.Show("A senha deve conter pelo menos uma letra mai�scula, uma letra min�scula, um n�mero e um caractere especial.");
                    return;
                }

                // Chama o RegisterService para cadastrar o usu�rio
                await registerService.CadastrarUsuario(input_email.Text, senha);
                MessageBox.Show("Usu�rio registrado com sucesso!");
            }
            catch (Exception ex)
            {
                // Trata exce��es e exibe mensagens de erro
                MessageBox.Show("Erro: " + ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Exce��o interna: " + ex.InnerException.Message);
                }
            }
        }

        // Fun��o para validar o formato do e-mail usando uma express�o regular
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // Fun��o para validar uma senha forte usando uma express�o regular
        private bool IsStrongPassword(string password)
        {
            // Pelo menos uma letra min�scula, uma letra mai�scula, um n�mero e um caractere especial, com comprimento m�nimo de 8 caracteres
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
        }

        // Fun��o para verificar se o e-mail j� existe no banco de dados
        private bool AlreadyExists(string email)
        {
            // Consulta o contexto do banco de dados para encontrar um usu�rio com o e-mail fornecido
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);

            // Se um usu�rio com o e-mail existir, retorna true; caso contr�rio, retorna false
            return existingUser != null;
        }
    }
}
