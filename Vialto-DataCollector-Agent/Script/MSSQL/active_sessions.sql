SELECT 
    session_id,
    status,
    command,
    blocking_session_id,
    wait_type,
    wait_time,
    wait_resource,
    total_elapsed_time,
    cpu_time,
    logical_reads,
    reads,
    writes,
    text AS query_text
FROM 
    sys.dm_exec_requests
CROSS APPLY 
    sys.dm_exec_sql_text(sql_handle);