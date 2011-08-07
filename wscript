
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    bld(features='d dstlib', source=aamSources( ), target='aam',
        dflags='-I..')
    bld(features='d dprogram', source=aamTestSources( ),
        target='AxisAndAlliesTroops.test', dflags='-unittest -I..', use='aam')
    bld(features='d dprogram', source='aam/main.d',
        target='AxisAndAlliesTroops', dflags='-I..', use='aam')

def aamSources( ):
    return ['aam/' + s for s in aamFiles( )]

def aamTestSources( ):
    return ['aam/test/' + s for s in aamTestFiles( )]

def aamTestFiles( ):
    return ['Test' + s for s in aamFiles( )] + ['Test.d', 'UnitTest.d']

def aamFiles( ):
    return ['Executor.d', 'Troop.d']
