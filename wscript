
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    bld(features='d dstlib', source='aam/Executor.d', target='aam',
        dflags='-I..')
    bld(features='d dprogram', source='aam/test/TestExecutor.d aam/test/Test.d',
        target='AxisAndAlliesTroops.test', dflags='-unittest -I..', use='aam')
    bld(features='d dprogram', source='aam/main.d',
        target='AxisAndAlliesTroops', dflags='-I..', use='aam')
