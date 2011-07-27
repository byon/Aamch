
def options(opt):
    opt.load('compiler_d waf_unit_test')

def configure(cnf):
    cnf.load('compiler_d waf_unit_test')

def build(bld):
    defineBuildForD(bld, 'AxisAndAlliesTroops/main.d', 'AamTroops')
    from waflib.Tools import waf_unit_test
    bld.add_post_fun(waf_unit_test.summary)

def defineBuildForD(target, sourceNames, targetName):
    target(features='d dprogram', source=sourceNames, target=targetName,
           dflags='-I..')
