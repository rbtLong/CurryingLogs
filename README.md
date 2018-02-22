# Currying Logs

This article is a tangent on Currying Databases.

*A little background*: Our student portal was riddled with bugs. Since we use a monolithic system
like ASP.Net Classic, any tiny issue could cause our entire page to crash. The codebase is riddled
with sloppy code handed down from person to person through 5 generations of programmers who formerly held
the position. These people had no interest in cleaning up the code or making it coherent. To make matters
worse, the vendor themselves had bugs that they did not fix.

Faculties and students would encounter bug sometimes and technicians could not replicate the issue. At times
there would be bugs that students knew, but no one reported it. These are some of the embarassing situations
were common-place on the student portal.

To prevent an all-out-page-crash, a RESTful approach was used (when possible). Breaking **gigantic** operations
into smaller units and wrapping each of those into a try-catch statements. Important operations that could
fail should be inside a try-catch statements (e.g. database operations). When catching the error, supply as much
information as possible such as variables and a simple and concise description of the error.

Currying is a great technique can be used to suite these purposes.

## How?

Currying allows us to string together expressions and execute them.

This is particularly useful when we need to supply important information inputs into a function.

For instance, this is a simple database call for an API route. If it fails, we get all the input variables that was used during the problem.

```C#

    const string cmdTemplate = "select * from _form_templates where id = @id and category = 'Course Forms (Pre-offer)' ";

    try
    {
        model = Db
            .Forms
            .Cmd(cmdTemplate)
            .Param("@id", info.formid)
            .Row();
    }
    catch (Exception ex)
    {
        ex.Error("[PZ Courses Edit] Error when trying to select the form templates.")
            .Add("cmdTemplate", cmdTemplate)
            .Add("model", model.Json())
            .Add("input", info.Json())
            .Ok();

        return ex.Handle();
    }

```

## What else?

Logging done correctly could eliminate hours or days of troubleshooting. A good logging system could allow the administrators to anticipate
potential issues before a huge one occurrs. Another useful aspect of logging is it can help provide information and trace when
necessary.

As an extra step, I created a service that would aggregate a list of errors within the past 24 hours at the end of the day. Looking at the
daily report enough times, a pattern begins to emerge. When a problem occurs, I can respond to it before the users complain. Sometimes, the
report can do checks or display patterns that are unusual and this can help us anticipate possible issues.