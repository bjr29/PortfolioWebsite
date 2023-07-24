using Api;
using MapDataReader;
using System.Data;
using System.Data.SqlClient;

const string sqlBasePath = "./Sql/";

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
		"/api/messages/{from:int},{count:int}",
		(int from, int count) => {
			using var database = new SqlConnection {
					ConnectionString = "Data Source=(localdb)\\PORTFOLIO",
			};

			database.Open();
			
			using var sqlCommand = new SqlCommand(GetMessagesSql(), database);

			sqlCommand.Parameters.Add("@from", SqlDbType.Int).Value = from;
			sqlCommand.Parameters.Add("@count", SqlDbType.Int).Value = count;

			return Results.Ok(sqlCommand.ExecuteReader().ToMessage());
		}
);

app.MapPost(
		"/api/messages/",
		(MessageParameter messageParameter) => {
			using var database = new SqlConnection {
					ConnectionString = "Data Source=(localdb)\\PORTFOLIO",
			};

			database.Open();
			
			using var sqlCommand = new SqlCommand(PostMessageSql(), database);

			sqlCommand.Parameters.Add("@nickname", SqlDbType.VarChar).Value = messageParameter.Nickname;
			sqlCommand.Parameters.Add("@contents", SqlDbType.VarChar).Value = messageParameter.Contents;
			sqlCommand.Parameters.Add("@time", SqlDbType.DateTime).Value = DateTime.Now;

			sqlCommand.ExecuteNonQuery();

			return Results.Ok();
		}
);

app.MapDelete(
		"/api/messages/{id:int}",
		(int id) => {
			using var database = new SqlConnection {
					ConnectionString = "Data Source=(localdb)\\PORTFOLIO",
			};

			database.Open();
			
			using var sqlCommand = new SqlCommand(DeleteMessageSql(), database);
			
			sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;

			sqlCommand.ExecuteNonQuery();

			return Results.Ok();
		}
);

app.Run();

string GetMessagesSql() {
	return File.ReadAllText($"{sqlBasePath}/GetMessages.sql");
}

string PostMessageSql() {
	return File.ReadAllText($"{sqlBasePath}/PostMessage.sql");
}

string DeleteMessageSql() {
	return File.ReadAllText($"{sqlBasePath}/DeleteMessage.sql");
}
