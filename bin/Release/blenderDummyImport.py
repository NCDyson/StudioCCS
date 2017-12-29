import bpy
from bpy.props import IntProperty, PointerProperty
from bpy.types import PropertyGroup, Operator, Panel
import os
import sys


dir = os.path.dirname(bpy.data.filepath)
if not dir in sys.path:
    sys.path.append(dir)

    
def makeDummy(name, position, rotation, options):
    tmpDummy = bpy.data.objects.get(name, None)
    if tmpDummy is not None:
        bpy.data.objects.remove(tmpDummy, True)
    
    tmpDummy = bpy.data.objects.new(name, object_data=None)
    bpy.context.scene.objects.link(tmpDummy)
    tmpDummy.empty_draw_size = 0.25
    tmpDummy.location = position
    tmpDummy.rotation_euler = rotation
    tmpDummy.rotation_mode = 'XYZ'
    tmpDummy.empty_draw_type = options["draw_type"]
    tmpDummy.show_name = options["show_names"]


class HackCCSDummyImport_SelectTextFile(Operator):
    bl_idname = "ccsdummy.selecttextfile"
    bl_options = {'REGISTER'}
    bl_label = "..."
    filepath = bpy.props.StringProperty(subtype="FILE_PATH")
    filter_glob = bpy.props.StringProperty(default='*.txt', options={'HIDDEN'})
    
    def execute(self, context):
        HackCCSDummyImport_ImportDummies.fileName = self.filepath
        return {'FINISHED'}

    def invoke(self, context, event):
        context.window_manager.fileselect_add(self)

        return {'RUNNING_MODAL'}


def doLoadTextFile(fileName, context, options):
    with open(fileName, "r") as inf:
        dummyCount = int(inf.readline())
        for i in range(dummyCount):
            tmpName = inf.readline().replace("\n","").replace("\r","")
            tmpPosStr = inf.readline().split("\t")
            tmpPos = (-float(tmpPosStr[0]), float(tmpPosStr[2]), float(tmpPosStr[1]))
            tmpRotStr = inf.readline().split("\t")
            tmpRot = (float(tmpRotStr[0]), float(tmpRotStr[1]), float(tmpRotStr[2]))
            makeDummy(tmpName, tmpPos, tmpRot, options)


    
class HackCCSDummyImport_ImportDummies(Operator):
    bl_idname = "hackimport.importdummies"
    bl_label = "Import .hack CCS Dummies from Text"
    
    fileName = ""
    options = {}
    
    def execute(self, context):
        doLoadTextFile(HackCCSDummyImport_ImportDummies.fileName, context, HackCCSDummyImport_ImportDummies.options)
        return {"FINISHED"}
        
    
class HackCCSDummyImportPanel(Panel):
    bl_idname = "hackimport.importdummypanel"
    bl_space_type = "VIEW_3D"
    bl_region_type = "TOOLS"
    bl_label = "Import Hack CCS Dummies From Text"
    bl_category = ".hack"
    
    def draw(self, context):
        scene = context.scene
        layout = self.layout
        textFileName = HackCCSDummyImport_ImportDummies.fileName
        row = layout.row(align=False)
        row.label("File:")
        row.alignment = 'EXPAND'
        row.label(textFileName)
        row.alignment = 'RIGHT'
        row.operator(HackCCSDummyImport_SelectTextFile.bl_idname)
        row = layout.row()
        HackCCSDummyImport_ImportDummies.options["draw_type"] = "CUBE"
        row.prop(scene.ccsdummyimport, 'showNames')
        HackCCSDummyImport_ImportDummies.options["show_names"] = scene.ccsdummyimport.showNames
            
        if(textFileName != ""):
            row = layout.row()
            row.operator(HackCCSDummyImport_ImportDummies.bl_idname)
        
        context.area.tag_redraw()
    
    
class HackCCSDummyImportSettings(bpy.types.PropertyGroup):
    showNames = bpy.props.BoolProperty(name="Show Names", description="Set Dummies to show names", default=True)
    
    
def register():
    #bpy.utils.register_module(__name__)
    bpy.utils.register_class(HackCCSDummyImportPanel)
    bpy.utils.register_class(HackCCSDummyImport_SelectTextFile)
    bpy.utils.register_class(HackCCSDummyImport_ImportDummies)
    bpy.utils.register_class(HackCCSDummyImportSettings)
    bpy.types.Scene.ccsdummyimport = bpy.props.PointerProperty(type=HackCCSDummyImportSettings)

    
def unregister():
    bpy.utils.unregister_class(HackCCSDummyImportPanel)
    bpy.utils.unregister_class(HackCCSDummyImport_SelectTextFile)
    bpy.utils.unregister_class(HackCCSDummyImport_ImportDummies)
    del(bpy.types.Scene.ccsdummyimport)
    bpy.utils.unregister_class(HackCCSDummyImportSettings)
    

if __name__ == "__main__":
    register()
