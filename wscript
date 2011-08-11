
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    bld.stlib(source=aamSources( ), target='aam', dflags='-I.. -g')
    bld.program(source=aamTestSources( ), target='AxisAndAlliesTroops.test',
                dflags='-unittest -I.. -g', use='aam')
    bld.program(source='aam/main.d', target='AxisAndAlliesTroops',
                dflags='-I.. -g', use='aam')

def aamSources( ):
    return ['aam/' + s for s in aamFiles( )]

def aamTestSources( ):
    return ['aam/test/' + s for s in aamTestFiles( )]

def aamTestFiles( ):
    return ['Test' + s for s in testedAamFiles( )] + ['Test.d', 'UnitTest.d']

def aamFiles( ):
    return testedAamFiles( ) + ['StartupException.d']

def testedAamFiles( ):
    return ['Executor.d', 'Troop.d', 'TroopsFromFile.d']
