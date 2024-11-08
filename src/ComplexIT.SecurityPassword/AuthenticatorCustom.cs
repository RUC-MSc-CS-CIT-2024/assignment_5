using Npgsql;

public class AuthenticatorCustom : Authenticator {
    public override bool passwordIsOK(string password, string username)
    {
        if(password.Length <= 8)
            return false;
        if(username != "admin" && password.Contains(username))
            return false;

        return true;
    }

    public override void sqlInsertUserRecord(NpgsqlCommand cmd, string username, string salt, string hashedpassword)
    {
        cmd.Parameters.AddRange(new NpgsqlParameter[] {
            new("p_user", username),
            new("p_pass", hashedpassword),
            new("p_salt", salt)
        });
        cmd.CommandText = "insert into password values (@p_user, @p_pass, @p_hash)";
    }

    public override void sqlSelectUserRecord(NpgsqlCommand cmd, string username)
    {
        cmd.Parameters.Add(new("p_username", username));
        cmd.CommandText = "SELECT salt, hashed_password FROM password WHERE username = @p_username";
    }
}
