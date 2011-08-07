
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    bld(features='d dstlib', source='aam/Executor.d aam/Troop.d', target='aam',
        dflags='-I..')
    bld(features='d dprogram', source='aam/test/TestTroop.d aam/test/Test.d aam/test/TestExecutor.d aam/test/UnitTest.d',
        target='AxisAndAlliesTroops.test', dflags='-unittest -I..', use='aam')
    bld(features='d dprogram', source='aam/main.d',
        target='AxisAndAlliesTroops', dflags='-I..', use='aam')
