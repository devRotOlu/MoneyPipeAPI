ALTER TABLE BackgroundJobs
ADD COLUMN Payload JSON;

ALTER TABLE BackgroundJobs
DROP COLUMN InvoiceId;

