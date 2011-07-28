
def options(opt):
    opt.load('compiler_d')

def configure(cnf):
    cnf.load('compiler_d')

def build(bld):
    defineBuildForD(bld, 'AxisAndAlliesTroops/main.d AxisAndAlliesTroops/Executor.d', 'AamTroops')

def defineBuildForD(build, sourceNames, targetName):
    build(features='d dprogram', source=sourceNames, target=targetName,
          dflags='-unittest -I..')
