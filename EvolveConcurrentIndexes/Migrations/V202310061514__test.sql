-- evolve-tx-off

CREATE UNIQUE INDEX CONCURRENTLY test_testname ON public.test(testname);

CREATE UNIQUE INDEX CONCURRENTLY test_testresult ON public.test(testresult);