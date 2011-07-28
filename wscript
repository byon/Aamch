
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    bld(features='d dstlib', source='aam/Executor.d', target='aam.test',
        dflags='-unittest -I..')
    bld(features='d dstlib', source='aam/Executor.d', target='aam',
        dflags='-I..')
    bld(features='d dprogram', source='aam/Test.d',
        target='AxisAndAlliesTroops.test', dflags='-I..', use='aam.test')
    bld(features='d dprogram', source='aam/main.d',
        target='AxisAndAlliesTroops', dflags='-I..', use='aam')
