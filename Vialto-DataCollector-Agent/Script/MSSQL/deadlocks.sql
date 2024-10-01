WITH XMLNAMESPACES ('http://schemas.microsoft.com/sqlserver/2004/07/showplan' AS p)
SELECT
    XEvent.query('(event/data/value)[1]').value('(./text())[1]', 'nvarchar(max)') AS DeadlockGraph
FROM 
    (SELECT XEvent.query('.') AS XEvent
     FROM 
         (SELECT CAST(target_data AS XML) AS TargetData
          FROM sys.dm_xe_session_targets AS xt
          JOIN sys.dm_xe_sessions AS xs ON xs.address = xt.event_session_address
          WHERE xs.name = 'system_health'
            AND xt.target_name = 'ring_buffer') AS Data
          CROSS APPLY TargetData.nodes('RingBufferTarget/event[@name="xml_deadlock_report"]') AS XEventData (XEvent)) AS Deadlocks;