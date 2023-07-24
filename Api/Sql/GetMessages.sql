select * from Messages
order by MessageID
offset @from rows
fetch next @count rows only;
