UPDATE users.tblUsers
SET [Password] = CONVERT(VARBINARY(MAX), '$2a$11$wrmX0I0yVxp1iskf3wiynepCMOWEjmVXvngHb.KQRSpx9qYkBUZZu')
WHERE UserId = 1;