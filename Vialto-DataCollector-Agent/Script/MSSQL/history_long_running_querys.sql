SELECT 
    qs.execution_type_desc,
    qs.query_id,
    qs.execution_count,
    qs.total_duration / 1000 AS total_duration_ms,
    qs.last_execution_time,
    qt.query_sql_text
FROM 
    sys.query_store_runtime_stats qs
JOIN 
    sys.query_store_query_text qt ON qs.query_id = qt.query_id
WHERE 
    qs.total_duration > 100000; -- Total duration in milliseconds, adjust this threshold
