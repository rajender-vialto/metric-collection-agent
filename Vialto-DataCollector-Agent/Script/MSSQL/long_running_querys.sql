SELECT 
    r.session_id,
    r.status,
    r.start_time,
    r.command,
    r.total_elapsed_time / 1000 AS elapsed_seconds,
    r.cpu_time / 1000 AS cpu_seconds,
    r.logical_reads,
    r.reads,
    r.writes,
    r.blocking_session_id,
    r.wait_type,
    r.wait_time / 1000 AS wait_seconds,
    r.wait_resource,
    DB_NAME(r.database_id) AS database_name,
    t.text AS query_text
FROM 
    sys.dm_exec_requests r
CROSS APPLY 
    sys.dm_exec_sql_text(r.sql_handle) AS t
WHERE 
    r.total_elapsed_time > 60000 -- Queries running for more than 60 seconds (adjust as needed)
ORDER BY 
    r.total_elapsed_time DESC;